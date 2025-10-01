using AttributeExporterXrmToolBoxPlugin.Models;
using AttributeExporterXrmToolBoxPlugin.Services;
using CsvHelper;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AttributeExporterXrmToolBoxPlugin
{
    public partial class AttributeExporterControl : PluginControlBase
    {
        private List<AttributeMetadataInfo> _allAttributes;
        private List<AttributeMetadataInfo> _filteredAttributes;
        private List<Models.SolutionInfo> _solutions;
        private ColumnConfiguration _columnConfiguration;
        private int _selectionAnchorRow = -1; // Tracks the anchor row for Shift+Click selection
        private bool _attributesLoaded = false;
        private string _lastLoadedState = null; // Tracks what was last loaded (solution ID or "AllEntities")

        public AttributeExporterControl()
        {
            InitializeComponent();
            _allAttributes = new List<AttributeMetadataInfo>();
            _filteredAttributes = new List<AttributeMetadataInfo>();
            _solutions = new List<Models.SolutionInfo>();

            // Load column configuration and build DataGridView columns
            _columnConfiguration = ColumnConfigurationService.LoadConfiguration();
            RebuildColumns(_columnConfiguration);

            // Filters intentionally not restored - start fresh each session
        }

        private void AttributeExporterControl_Load(object sender, EventArgs e)
        {
            UpdateConnectionState();
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            UpdateConnectionState();
        }

        private void UpdateConnectionState()
        {
            bool hasConnection = Service != null;

            // Update button states
            btnExport.Enabled = hasConnection && _allAttributes.Count > 0;
            btnReloadSolutions.Enabled = hasConnection && rdoSelectedSolution.Checked;

            // Show/hide connection message (informational only)
            lblConnectionMessage.Visible = !hasConnection;

            if (hasConnection)
            {
                LoadSolutions();
            }
        }

        private void UpdateLoadAttributesButtonText()
        {
            // Determine current state
            string currentState = rdoAllEntities.Checked ? "AllEntities" : cboSolutions.SelectedValue?.ToString();

            // If attributes have been loaded and the state hasn't changed, show "Reload"
            if (_attributesLoaded && currentState == _lastLoadedState)
            {
                btnLoadAttributes.Text = "Reload Attributes";
            }
            else
            {
                btnLoadAttributes.Text = "Load Attributes";
            }
        }

        private void LoadSolutions()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions...",
                Work = (worker, args) =>
                {
                    // Query 1: Get all visible solutions (managed and unmanaged)
                    var query = new QueryExpression("solution")
                    {
                        ColumnSet = new ColumnSet("solutionid", "friendlyname", "uniquename", "version"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("isvisible", ConditionOperator.Equal, true)
                            }
                        }
                    };
                    query.Orders.Add(new OrderExpression("friendlyname", OrderType.Ascending));

                    var solutions = Service.RetrieveMultiple(query);

                    // Query 2: Get solution IDs that contain attributes (componenttype = 2)
                    // This ensures only solutions with exportable attribute metadata appear in dropdown
                    var componentQuery = new QueryExpression("solutioncomponent")
                    {
                        ColumnSet = new ColumnSet("solutionid"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("componenttype", ConditionOperator.Equal, 2) // 2 = Attribute
                            }
                        },
                        Distinct = true
                    };

                    var componentsWithAttributes = Service.RetrieveMultiple(componentQuery);
                    var solutionIdsWithAttributes = componentsWithAttributes.Entities
                        .Select(e => e.GetAttributeValue<EntityReference>("solutionid")?.Id ?? Guid.Empty)
                        .Where(id => id != Guid.Empty)
                        .ToHashSet();

                    // Filter solutions to only those with attributes
                    var filteredSolutions = solutions.Entities
                        .Where(e => solutionIdsWithAttributes.Contains(e.Id))
                        .Select(e => new Models.SolutionInfo
                        {
                            Id = e.Id,
                            Name = e.GetAttributeValue<string>("friendlyname"),
                            UniqueName = e.GetAttributeValue<string>("uniquename"),
                            Version = e.GetAttributeValue<string>("version")
                        })
                        .ToList();

                    args.Result = filteredSolutions;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show($"Error loading solutions: {args.Error.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _solutions = (List<Models.SolutionInfo>)args.Result;
                    cboSolutions.DataSource = _solutions;
                    cboSolutions.DisplayMember = "Name";
                    cboSolutions.ValueMember = "Id";
                }
            });
        }

        private void RebuildColumns(ColumnConfiguration config)
        {
            dgvAttributes.AutoGenerateColumns = false;
            dgvAttributes.AllowUserToAddRows = false;
            dgvAttributes.AllowUserToDeleteRows = false;
            dgvAttributes.ReadOnly = true;
            dgvAttributes.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvAttributes.MultiSelect = true;
            dgvAttributes.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            // Enable advanced features
            dgvAttributes.AllowUserToOrderColumns = true;
            dgvAttributes.ScrollBars = ScrollBars.Both;
            dgvAttributes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Unsubscribe from events before clearing
            dgvAttributes.ColumnDisplayIndexChanged -= dgvAttributes_ColumnDisplayIndexChanged;
            dgvAttributes.ColumnWidthChanged -= dgvAttributes_ColumnWidthChanged;
            dgvAttributes.Sorted -= dgvAttributes_Sorted;
            dgvAttributes.CellMouseDown -= dgvAttributes_CellMouseDown;
            dgvAttributes.CellDoubleClick -= dgvAttributes_CellDoubleClick;
            dgvAttributes.SelectionChanged -= dgvAttributes_SelectionChanged;

            // Clear existing columns
            dgvAttributes.Columns.Clear();

            // Add visible columns from configuration
            var visibleColumns = config.Columns
                .Where(c => c.IsVisible)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            foreach (var colDef in visibleColumns)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = colDef.Name,
                    HeaderText = colDef.DisplayName,
                    DataPropertyName = colDef.Name,
                    Width = colDef.Width ?? 150,
                    SortMode = DataGridViewColumnSortMode.Automatic,
                    ToolTipText = colDef.Description
                };

                dgvAttributes.Columns.Add(column);
            }

            // Subscribe to events
            dgvAttributes.ColumnDisplayIndexChanged += dgvAttributes_ColumnDisplayIndexChanged;
            dgvAttributes.ColumnWidthChanged += dgvAttributes_ColumnWidthChanged;
            dgvAttributes.Sorted += dgvAttributes_Sorted;
            dgvAttributes.CellMouseDown += dgvAttributes_CellMouseDown;
            dgvAttributes.CellDoubleClick += dgvAttributes_CellDoubleClick;
            dgvAttributes.SelectionChanged += dgvAttributes_SelectionChanged;

            // Restore sort state if available
            // Note: We don't programmatically sort here because DataGridView.Sort() requires IBindingList
            // The sort glyph and state will be restored when user clicks a column header
            if (!string.IsNullOrEmpty(config.LastSortColumn))
            {
                var sortColumn = dgvAttributes.Columns[config.LastSortColumn];
                if (sortColumn != null && dgvAttributes.DataSource != null)
                {
                    // If we need to restore sort, re-apply it to the data source itself
                    var sortedData = _allAttributes.AsEnumerable();
                    var prop = typeof(AttributeMetadataInfo).GetProperty(config.LastSortColumn);
                    if (prop != null)
                    {
                        sortedData = config.LastSortAscending
                            ? sortedData.OrderBy(x => prop.GetValue(x))
                            : sortedData.OrderByDescending(x => prop.GetValue(x));
                        dgvAttributes.DataSource = sortedData.ToList();
                    }
                }
            }
        }

        private void dgvAttributes_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Update display order in configuration based on current DisplayIndex
            var visibleColumns = _columnConfiguration.Columns.Where(c => c.IsVisible).ToList();
            foreach (DataGridViewColumn col in dgvAttributes.Columns)
            {
                var colDef = visibleColumns.FirstOrDefault(c => c.Name == col.Name);
                if (colDef != null)
                {
                    colDef.DisplayOrder = col.DisplayIndex;
                }
            }
            ColumnConfigurationService.SaveConfiguration(_columnConfiguration);
        }

        private void dgvAttributes_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Update column width in configuration
            var colDef = _columnConfiguration.Columns.FirstOrDefault(c => c.Name == e.Column.Name);
            if (colDef != null)
            {
                colDef.Width = e.Column.Width;
                ColumnConfigurationService.SaveConfiguration(_columnConfiguration);
            }
        }

        private void dgvAttributes_Sorted(object sender, EventArgs e)
        {
            // Save sort state
            if (dgvAttributes.SortedColumn != null)
            {
                _columnConfiguration.LastSortColumn = dgvAttributes.SortedColumn.Name;
                _columnConfiguration.LastSortAscending = dgvAttributes.SortOrder == SortOrder.Ascending;
                ColumnConfigurationService.SaveConfiguration(_columnConfiguration);
            }
        }

        private void dgvAttributes_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Only handle row header clicks (column index -1)
            if (e.ColumnIndex != -1 || e.RowIndex < 0 || e.Button != MouseButtons.Left)
                return;

            // Check for modifier keys
            bool ctrlPressed = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            bool shiftPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            if (shiftPressed && _selectionAnchorRow >= 0 && _selectionAnchorRow < dgvAttributes.Rows.Count)
            {
                // Shift+Click: Select range from anchor row to clicked row
                int startRow = Math.Min(_selectionAnchorRow, e.RowIndex);
                int endRow = Math.Max(_selectionAnchorRow, e.RowIndex);

                dgvAttributes.ClearSelection();
                for (int row = startRow; row <= endRow; row++)
                {
                    foreach (DataGridViewCell cell in dgvAttributes.Rows[row].Cells)
                    {
                        cell.Selected = true;
                    }
                }

                // DON'T set CurrentCell during Shift+Click - it would clear our selection
                // The arrow indicator stays at the anchor row
            }
            else
            {
                // Set the current cell FIRST (moves the arrow indicator)
                if (dgvAttributes.Rows[e.RowIndex].Cells.Count > 0)
                {
                    dgvAttributes.CurrentCell = dgvAttributes.Rows[e.RowIndex].Cells[0];
                }

                if (ctrlPressed)
                {
                    // Ctrl+Click: Toggle row selection
                    bool isRowSelected = dgvAttributes.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().Any(c => c.Selected);

                    foreach (DataGridViewCell cell in dgvAttributes.Rows[e.RowIndex].Cells)
                    {
                        cell.Selected = !isRowSelected;
                    }
                }
                else
                {
                    // Normal click: Select only this row
                    dgvAttributes.ClearSelection();
                    foreach (DataGridViewCell cell in dgvAttributes.Rows[e.RowIndex].Cells)
                    {
                        cell.Selected = true;
                    }
                }

                // Update anchor row for future Shift+Click operations
                _selectionAnchorRow = e.RowIndex;
            }
        }

        private void dgvAttributes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header row clicks
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            try
            {
                var cell = dgvAttributes.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var columnName = dgvAttributes.Columns[e.ColumnIndex].HeaderText;
                var cellValue = cell.Value?.ToString() ?? "(empty)";

                // Show custom dialog with copy button
                using (var dialog = new Form())
                {
                    dialog.Text = columnName;
                    dialog.Width = 500;
                    dialog.Height = 350;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.FormBorderStyle = FormBorderStyle.Sizable;
                    dialog.MinimizeBox = false;
                    dialog.MaximizeBox = false;

                    // TextBox to display value
                    var textBox = new TextBox
                    {
                        Multiline = true,
                        ReadOnly = true,
                        ScrollBars = ScrollBars.Both,
                        Text = cellValue,
                        Dock = DockStyle.Fill,
                        Font = new System.Drawing.Font("Segoe UI", 9F),
                        BorderStyle = BorderStyle.None,
                        BackColor = System.Drawing.SystemColors.Window
                    };

                    // Panel for buttons
                    var buttonPanel = new Panel
                    {
                        Height = 40,
                        Dock = DockStyle.Bottom,
                        Padding = new Padding(10)
                    };

                    // Copy button
                    var btnCopy = new Button
                    {
                        Text = "Copy Text",
                        Width = 90,
                        Height = 30,
                        Location = new System.Drawing.Point(10, 5),
                        DialogResult = DialogResult.None
                    };
                    btnCopy.Click += (s, args) =>
                    {
                        Clipboard.SetText(cellValue);
                        btnCopy.Text = "Copied!";
                        Task.Delay(1000).ContinueWith(t =>
                        {
                            if (!btnCopy.IsDisposed)
                            {
                                btnCopy.Invoke(new Action(() => btnCopy.Text = "Copy Text"));
                            }
                        });
                    };

                    // OK button
                    var btnOk = new Button
                    {
                        Text = "OK",
                        Width = 80,
                        Height = 30,
                        Location = new System.Drawing.Point(110, 5),
                        DialogResult = DialogResult.OK
                    };

                    buttonPanel.Controls.Add(btnCopy);
                    buttonPanel.Controls.Add(btnOk);

                    // Panel for textbox with padding
                    var textPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        Padding = new Padding(10)
                    };
                    textPanel.Controls.Add(textBox);

                    dialog.Controls.Add(textPanel);
                    dialog.Controls.Add(buttonPanel);
                    dialog.AcceptButton = btnOk;

                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying cell value: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAttributes_SelectionChanged(object sender, EventArgs e)
        {
            // Count unique rows that have at least one selected cell
            var selectedRowCount = dgvAttributes.SelectedCells.Cast<DataGridViewCell>()
                .Select(c => c.RowIndex)
                .Distinct()
                .Count();

            lblAttributeCount.Text = $"Selected Attributes: {selectedRowCount}";
        }

        private void rdoSelectedSolution_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSelectedSolution.Checked)
            {
                lblSolution.Enabled = true;
                cboSolutions.Enabled = true;
                btnReloadSolutions.Enabled = Service != null;
                UpdateLoadAttributesButtonText();
            }
        }

        private void rdoAllEntities_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllEntities.Checked)
            {
                lblSolution.Enabled = false;
                cboSolutions.Enabled = false;
                cboSolutions.SelectedIndex = -1; // Clear selection
                btnReloadSolutions.Enabled = false;
                UpdateLoadAttributesButtonText();
            }
        }

        private void cboSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLoadAttributesButtonText();
        }

        private void btnLoadAttributes_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadAttributesInternal);
        }

        private void LoadAttributesInternal()
        {
            if (rdoAllEntities.Checked)
            {
                LoadAllEntities();
            }
            else if (rdoSelectedSolution.Checked)
            {
                if (cboSolutions.SelectedItem == null)
                {
                    MessageBox.Show("Please select a solution first.", "Selection Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedSolution = (Models.SolutionInfo)cboSolutions.SelectedItem;
                LoadAttributesForSolution(selectedSolution);
            }
        }

        private void LoadAllEntities()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading all entities and attributes...",
                Work = (worker, args) =>
                {
                    var retrieveAllEntitiesRequest = new RetrieveAllEntitiesRequest
                    {
                        EntityFilters = EntityFilters.Attributes,
                        RetrieveAsIfPublished = true
                    };

                    var retrieveAllEntitiesResponse = (RetrieveAllEntitiesResponse)Service.Execute(retrieveAllEntitiesRequest);
                    var attributes = new List<AttributeMetadataInfo>();

                    foreach (var entity in retrieveAllEntitiesResponse.EntityMetadata)
                    {
                        try
                        {
                            foreach (var attribute in entity.Attributes)
                            {
                                if (attribute.IsValidForRead == true)
                                {
                                    attributes.Add(ConvertToAttributeInfo(entity, attribute));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log error but continue
                            Console.WriteLine($"Error processing entity {entity.LogicalName}: {ex.Message}");
                        }
                    }
                    args.Result = attributes;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show($"Error loading attributes: {args.Error.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _allAttributes = (List<AttributeMetadataInfo>)args.Result;
                    _filteredAttributes = new List<AttributeMetadataInfo>(_allAttributes);

                    // Populate filter type dropdown with distinct types from loaded data
                    PopulateFilterTypeComboBox();

                    // Clear filters on new load
                    ClearAllFilters();

                    RefreshGrid();

                    // Update state tracking for dynamic button text
                    _attributesLoaded = true;
                    _lastLoadedState = "AllEntities";
                    UpdateLoadAttributesButtonText();

                    MessageBox.Show($"Successfully loaded {_allAttributes.Count} attributes from all entities",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }

        private void LoadAttributesForSolution(Models.SolutionInfo solution)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Loading attributes for solution '{solution.Name}'...",
                Work = (worker, args) =>
                {
                    // Query for ATTRIBUTES (componenttype = 2) in this solution
                    var query = new QueryExpression("solutioncomponent")
                    {
                        ColumnSet = new ColumnSet("objectid"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("solutionid", ConditionOperator.Equal, solution.Id),
                                new ConditionExpression("componenttype", ConditionOperator.Equal, 2) // 2 = Attribute
                            }
                        }
                    };

                    var solutionComponents = Service.RetrieveMultiple(query);
                    var attributeMetadataIds = solutionComponents.Entities
                        .Select(e => e.GetAttributeValue<Guid>("objectid"))
                        .ToList();

                    var attributes = new List<AttributeMetadataInfo>();

                    if (attributeMetadataIds.Count == 0)
                    {
                        args.Result = attributes;
                        return;
                    }

                    // OPTIMIZATION: Batch retrieve all attributes using ExecuteMultipleRequest
                    var executeMultipleRequest = new ExecuteMultipleRequest
                    {
                        Requests = new OrganizationRequestCollection(),
                        Settings = new ExecuteMultipleSettings
                        {
                            ContinueOnError = true,
                            ReturnResponses = true
                        }
                    };

                    // Add all attribute retrieval requests to the batch
                    foreach (var metadataId in attributeMetadataIds)
                    {
                        executeMultipleRequest.Requests.Add(new RetrieveAttributeRequest
                        {
                            MetadataId = metadataId,
                            RetrieveAsIfPublished = true
                        });
                    }

                    // Execute all attribute retrievals in one batch
                    var executeMultipleResponse = (ExecuteMultipleResponse)Service.Execute(executeMultipleRequest);

                    // Collect all successfully retrieved attributes
                    var retrievedAttributes = new List<AttributeMetadata>();
                    foreach (var responseItem in executeMultipleResponse.Responses)
                    {
                        if (responseItem.Response != null)
                        {
                            var attrResponse = (RetrieveAttributeResponse)responseItem.Response;
                            retrievedAttributes.Add(attrResponse.AttributeMetadata);
                        }
                    }

                    // OPTIMIZATION: Cache entity metadata (retrieve each unique entity only once)
                    var entityCache = new Dictionary<string, EntityMetadata>();
                    var uniqueEntityNames = retrievedAttributes
                        .Select(a => a.EntityLogicalName)
                        .Distinct()
                        .ToList();

                    foreach (var entityName in uniqueEntityNames)
                    {
                        try
                        {
                            var entityRequest = new RetrieveEntityRequest
                            {
                                LogicalName = entityName,
                                EntityFilters = EntityFilters.Entity,
                                RetrieveAsIfPublished = true
                            };

                            var entityResponse = (RetrieveEntityResponse)Service.Execute(entityRequest);
                            entityCache[entityName] = entityResponse.EntityMetadata;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error retrieving entity {entityName}: {ex.Message}");
                        }
                    }

                    // Build results using cached entity metadata
                    foreach (var attributeMetadata in retrievedAttributes)
                    {
                        try
                        {
                            if (attributeMetadata.IsValidForRead == true &&
                                entityCache.TryGetValue(attributeMetadata.EntityLogicalName, out var entityMetadata))
                            {
                                attributes.Add(ConvertToAttributeInfo(entityMetadata, attributeMetadata));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error converting attribute {attributeMetadata.LogicalName}: {ex.Message}");
                        }
                    }

                    args.Result = attributes;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show($"Error loading attributes: {args.Error.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _allAttributes = (List<AttributeMetadataInfo>)args.Result;
                    _filteredAttributes = new List<AttributeMetadataInfo>(_allAttributes);

                    // Populate filter type dropdown with distinct types from loaded data
                    PopulateFilterTypeComboBox();

                    // Clear filters on new load
                    ClearAllFilters();

                    RefreshGrid();

                    // Update state tracking for dynamic button text
                    _attributesLoaded = true;
                    _lastLoadedState = solution.Id.ToString();
                    UpdateLoadAttributesButtonText();

                    MessageBox.Show($"Successfully loaded {_allAttributes.Count} attributes from solution '{solution.Name}'",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }

        private string GetEntityLogicalName(Guid entityId)
        {
            try
            {
                var entity = Service.Retrieve("entity", entityId, new ColumnSet("logicalname"));
                return entity.GetAttributeValue<string>("logicalname");
            }
            catch
            {
                return null;
            }
        }

        private AttributeMetadataInfo ConvertToAttributeInfo(EntityMetadata entity, AttributeMetadata attribute)
        {
            return new AttributeMetadataInfo
            {
                // Core Identification
                TableLogicalName = entity.LogicalName,
                TableDisplayName = entity.DisplayName?.UserLocalizedLabel?.Label ?? entity.LogicalName,
                AttributeLogicalName = attribute.LogicalName,
                AttributeDisplayName = attribute.DisplayName?.UserLocalizedLabel?.Label ?? attribute.LogicalName,
                SchemaName = attribute.SchemaName,
                AttributeType = attribute.AttributeType?.ToString() ?? "Unknown",
                AttributeTypeName = attribute.AttributeTypeName?.Value,

                // Metadata Properties
                MetadataId = attribute.MetadataId?.ToString(),
                IntroducedVersion = attribute.IntroducedVersion,
                IsManaged = attribute.IsManaged,
                IsCustomAttribute = attribute.IsCustomAttribute,

                // Field Characteristics
                Required = attribute.RequiredLevel?.Value == AttributeRequiredLevel.ApplicationRequired,
                RequiredLevel = attribute.RequiredLevel?.Value.ToString(),
                MaxLength = GetMaxLength(attribute),
                Description = attribute.Description?.UserLocalizedLabel?.Label ?? "",

                // Identity Flags
                IsPrimaryId = attribute.IsPrimaryId,
                IsPrimaryName = attribute.IsPrimaryName,
                IsLogical = attribute.IsLogical,
                AttributeOf = attribute.AttributeOf,

                // Validity Flags
                IsValidForCreate = attribute.IsValidForCreate,
                IsValidForRead = attribute.IsValidForRead,
                IsValidForUpdate = attribute.IsValidForUpdate,

                // Security
                IsSecured = attribute.IsSecured,
                CanBeSecuredForCreate = attribute.CanBeSecuredForCreate,
                CanBeSecuredForRead = attribute.CanBeSecuredForRead,
                CanBeSecuredForUpdate = attribute.CanBeSecuredForUpdate,

                // Audit
                IsAuditEnabled = attribute.IsAuditEnabled?.Value,

                // Type-Specific Details (Smart Column)
                TypeSpecificDetails = MetadataFormatter.GetTypeSpecificDetails(attribute)
            };
        }

        private int? GetMaxLength(AttributeMetadata attribute)
        {
            switch (attribute)
            {
                case StringAttributeMetadata stringAttr:
                    return stringAttr.MaxLength;
                case MemoAttributeMetadata memoAttr:
                    return memoAttr.MaxLength;
                default:
                    return null;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExecuteMethod(ExportInternal);
        }

        private void ExportInternal()
        {
            if (_allAttributes.Count == 0)
            {
                MessageBox.Show("No attributes to export. Please load attributes first.",
                    "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveDialog.FileName = $"AttributeMetadata_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(saveDialog.FileName);
                }
            }
        }

        private void ApplyFilter()
        {
            var filtered = _allAttributes.AsEnumerable();

            // Apply filters
            if (_columnConfiguration?.ActiveFilters != null)
            {
                var filters = _columnConfiguration.ActiveFilters;

                // Table name filter (searches both logical and display name)
                if (!string.IsNullOrWhiteSpace(filters.TableName))
                {
                    var tableFilter = filters.TableName.ToLower();
                    filtered = filtered.Where(a =>
                        (a.TableLogicalName?.ToLower().Contains(tableFilter) ?? false) ||
                        (a.TableDisplayName?.ToLower().Contains(tableFilter) ?? false)
                    );
                }

                // Attribute name filter (searches both logical and display name)
                if (!string.IsNullOrWhiteSpace(filters.AttributeName))
                {
                    var attrFilter = filters.AttributeName.ToLower();
                    filtered = filtered.Where(a =>
                        (a.AttributeLogicalName?.ToLower().Contains(attrFilter) ?? false) ||
                        (a.AttributeDisplayName?.ToLower().Contains(attrFilter) ?? false)
                    );
                }

                // Type filter
                if (!string.IsNullOrWhiteSpace(filters.AttributeType) && filters.AttributeType != "All")
                {
                    filtered = filtered.Where(a => a.AttributeType == filters.AttributeType);
                }

                // Required filter
                if (!string.IsNullOrWhiteSpace(filters.Required) && filters.Required != "All")
                {
                    bool requiredValue = filters.Required == "Yes";
                    filtered = filtered.Where(a => a.Required == requiredValue);
                }

                // IsCustom filter
                if (!string.IsNullOrWhiteSpace(filters.IsCustom) && filters.IsCustom != "All")
                {
                    bool customValue = filters.IsCustom == "Yes";
                    filtered = filtered.Where(a => a.IsCustomAttribute == customValue);
                }

                // IsPrimaryId filter
                if (!string.IsNullOrWhiteSpace(filters.IsPrimaryId) && filters.IsPrimaryId != "All")
                {
                    bool primaryIdValue = filters.IsPrimaryId == "Yes";
                    filtered = filtered.Where(a => a.IsPrimaryId == primaryIdValue);
                }
            }

            _filteredAttributes = filtered.ToList();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvAttributes.DataSource = null;
            dgvAttributes.DataSource = _filteredAttributes;
            lblFilterStatus.Text = $"Total Attributes: {_allAttributes.Count}";
            lblAttributeCount.Text = "Selected Attributes: 0";
            btnExport.Enabled = _filteredAttributes.Count > 0;
        }

        private void ExportToCsv(string filePath)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Exporting to CSV...",
                Work = (worker, args) =>
                {
                    // Get visible columns in display order
                    var visibleColumns = _columnConfiguration.Columns
                        .Where(c => c.IsVisible)
                        .OrderBy(c => c.DisplayOrder)
                        .ToList();

                    using (var writer = new StreamWriter(filePath))
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.Configuration.CultureInfo = System.Globalization.CultureInfo.InvariantCulture;

                        // Write headers
                        foreach (var col in visibleColumns)
                        {
                            csv.WriteField(col.DisplayName);
                        }
                        csv.NextRecord();

                        // Write data rows
                        foreach (var attr in _filteredAttributes)
                        {
                            foreach (var col in visibleColumns)
                            {
                                var propInfo = typeof(AttributeMetadataInfo).GetProperty(col.Name);
                                var value = propInfo?.GetValue(attr);
                                csv.WriteField(value?.ToString() ?? "");
                            }
                            csv.NextRecord();
                        }
                    }
                    args.Result = filePath;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show($"Error exporting to CSV: {args.Error.Message}", "Export Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show($"Successfully exported {_filteredAttributes.Count} attributes to:\n{args.Result}",
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }

        private void btnColumns_Click(object sender, EventArgs e)
        {
            using (var dialog = new Forms.ColumnConfigurationDialog(_columnConfiguration))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _columnConfiguration = dialog.Configuration;
                    ColumnConfigurationService.SaveConfiguration(_columnConfiguration);
                    RebuildColumns(_columnConfiguration);
                    RefreshGrid();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // The Close button is part of the main tool, not the plugin control
            // This button might not be needed anymore, but leaving the event handler for now
            // In XrmToolBox, the parent tool window handles closing.
            ParentForm?.Close();
        }

        private void btnReloadSolutions_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadSolutions);
        }

        #region Filter Events

        private void FilterControl_Changed(object sender, EventArgs e)
        {
            // Update filter criteria from controls
            if (_columnConfiguration?.ActiveFilters != null)
            {
                _columnConfiguration.ActiveFilters.TableName = txtFilterTable.Text;
                _columnConfiguration.ActiveFilters.AttributeName = txtFilterAttribute.Text;
                _columnConfiguration.ActiveFilters.AttributeType = cboFilterType.SelectedItem?.ToString() ?? "All";
                _columnConfiguration.ActiveFilters.Required = cboFilterRequired.SelectedItem?.ToString() ?? "All";
                _columnConfiguration.ActiveFilters.IsCustom = cboFilterCustom.SelectedItem?.ToString() ?? "All";
                _columnConfiguration.ActiveFilters.IsPrimaryId = cboFilterPrimaryId.SelectedItem?.ToString() ?? "All";

                // Filters are not persisted to disk - only active during session

                // Apply filters
                ApplyFilter();
            }
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            ClearAllFilters();
        }

        private void ClearAllFilters()
        {
            // Reset all filter controls
            txtFilterTable.Text = string.Empty;
            txtFilterAttribute.Text = string.Empty;

            // Only set combo box if it has items
            if (cboFilterType.Items.Count > 0)
                cboFilterType.SelectedIndex = 0; // Select "All"
            if (cboFilterRequired.Items.Count > 0)
                cboFilterRequired.SelectedIndex = 0; // Select "All"
            if (cboFilterCustom.Items.Count > 0)
                cboFilterCustom.SelectedIndex = 0; // Select "All"
            if (cboFilterPrimaryId.Items.Count > 0)
                cboFilterPrimaryId.SelectedIndex = 0; // Select "All"

            // Reset filter criteria in memory
            if (_columnConfiguration?.ActiveFilters != null)
            {
                _columnConfiguration.ActiveFilters.Reset();
            }

            // Apply filters (which will now show all results)
            if (_allAttributes.Count > 0)
            {
                ApplyFilter();
            }
        }

        private void PopulateFilterTypeComboBox()
        {
            // Get distinct attribute types from loaded data
            var distinctTypes = _allAttributes
                .Select(a => a.AttributeType)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            cboFilterType.Items.Clear();
            cboFilterType.Items.Add("All");
            foreach (var type in distinctTypes)
            {
                cboFilterType.Items.Add(type);
            }
            cboFilterType.SelectedIndex = 0;
        }

        private void RestoreFilterState()
        {
            // Restore filter values
            if (_columnConfiguration.ActiveFilters != null)
            {
                txtFilterTable.Text = _columnConfiguration.ActiveFilters.TableName ?? string.Empty;
                txtFilterAttribute.Text = _columnConfiguration.ActiveFilters.AttributeName ?? string.Empty;

                // Set combo box values
                SetComboBoxValue(cboFilterType, _columnConfiguration.ActiveFilters.AttributeType);
                SetComboBoxValue(cboFilterRequired, _columnConfiguration.ActiveFilters.Required);
                SetComboBoxValue(cboFilterCustom, _columnConfiguration.ActiveFilters.IsCustom);
                SetComboBoxValue(cboFilterPrimaryId, _columnConfiguration.ActiveFilters.IsPrimaryId);
            }

            // Initialize default values for combo boxes if not set (and they have items)
            if (cboFilterRequired.Items.Count > 0 && cboFilterRequired.SelectedIndex == -1)
                cboFilterRequired.SelectedIndex = 0;
            if (cboFilterCustom.Items.Count > 0 && cboFilterCustom.SelectedIndex == -1)
                cboFilterCustom.SelectedIndex = 0;
            if (cboFilterPrimaryId.Items.Count > 0 && cboFilterPrimaryId.SelectedIndex == -1)
                cboFilterPrimaryId.SelectedIndex = 0;
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            // Don't set SelectedIndex if combo box has no items
            if (comboBox.Items.Count == 0)
                return;

            if (string.IsNullOrEmpty(value) || value == "All")
            {
                comboBox.SelectedIndex = 0;
            }
            else
            {
                int index = comboBox.Items.IndexOf(value);
                if (index >= 0)
                {
                    comboBox.SelectedIndex = index;
                }
                else
                {
                    comboBox.SelectedIndex = 0;
                }
            }
        }

        #endregion

        #region Context Menu Events

        private void cmsGridContext_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Update menu item text based on selection count
            int selectedCellCount = dgvAttributes.SelectedCells.Count;
            int selectedRowCount = dgvAttributes.SelectedCells.Cast<DataGridViewCell>()
                .Select(c => c.RowIndex)
                .Distinct()
                .Count();

            tsmCopyCells.Text = selectedCellCount == 1 ? "Copy Cell" : $"Copy Cell(s) ({selectedCellCount})";
            tsmCopyFullRows.Text = selectedRowCount == 1 ? "Copy Full Row" : $"Copy Full Row(s) ({selectedRowCount})";
        }

        private void tsmCopyCells_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAttributes.SelectedCells.Count == 0)
                    return;

                // Use the built-in clipboard copy functionality
                Clipboard.SetDataObject(dgvAttributes.GetClipboardContent());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying cells: {ex.Message}", "Copy Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmCopyFullRows_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAttributes.SelectedCells.Count == 0)
                    return;

                // Get unique rows from selected cells
                var selectedRows = dgvAttributes.SelectedCells.Cast<DataGridViewCell>()
                    .Select(c => c.RowIndex)
                    .Distinct()
                    .OrderBy(r => r)
                    .ToList();

                var result = new System.Text.StringBuilder();

                foreach (var rowIndex in selectedRows)
                {
                    var row = dgvAttributes.Rows[rowIndex];
                    var values = new List<string>();

                    // Get all visible column values
                    foreach (DataGridViewColumn col in dgvAttributes.Columns)
                    {
                        if (col.Visible)
                        {
                            var cellValue = row.Cells[col.Index].Value;
                            values.Add(cellValue?.ToString() ?? "");
                        }
                    }

                    result.AppendLine(string.Join("\t", values));
                }

                Clipboard.SetText(result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying rows: {ex.Message}", "Copy Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
