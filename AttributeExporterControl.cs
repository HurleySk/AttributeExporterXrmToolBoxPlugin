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

        public AttributeExporterControl()
        {
            InitializeComponent();
            _allAttributes = new List<AttributeMetadataInfo>();
            _filteredAttributes = new List<AttributeMetadataInfo>();
            _solutions = new List<Models.SolutionInfo>();

            // Load column configuration and build DataGridView columns
            _columnConfiguration = ColumnConfigurationService.LoadConfiguration();
            RebuildColumns(_columnConfiguration);
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

            // Update export button state
            btnExport.Enabled = hasConnection && _allAttributes.Count > 0;

            // Show/hide connection message (informational only)
            lblConnectionMessage.Visible = !hasConnection;

            if (hasConnection)
            {
                LoadSolutions();
            }
        }

        private void LoadSolutions()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions...",
                Work = (worker, args) =>
                {
                    var query = new QueryExpression("solution")
                    {
                        ColumnSet = new ColumnSet("solutionid", "friendlyname", "uniquename", "version"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("isvisible", ConditionOperator.Equal, true),
                                new ConditionExpression("ismanaged", ConditionOperator.Equal, false)
                            }
                        }
                    };
                    query.Orders.Add(new OrderExpression("friendlyname", OrderType.Ascending));

                    var solutions = Service.RetrieveMultiple(query);
                    args.Result = solutions.Entities.Select(e => new Models.SolutionInfo
                    {
                        Id = e.Id,
                        Name = e.GetAttributeValue<string>("friendlyname"),
                        UniqueName = e.GetAttributeValue<string>("uniquename"),
                        Version = e.GetAttributeValue<string>("version")
                    }).ToList();
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
            dgvAttributes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttributes.MultiSelect = true;

            // Enable advanced features
            dgvAttributes.AllowUserToOrderColumns = true;
            dgvAttributes.ScrollBars = ScrollBars.Both;
            dgvAttributes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Unsubscribe from events before clearing
            dgvAttributes.ColumnDisplayIndexChanged -= dgvAttributes_ColumnDisplayIndexChanged;
            dgvAttributes.ColumnWidthChanged -= dgvAttributes_ColumnWidthChanged;
            dgvAttributes.Sorted -= dgvAttributes_Sorted;

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

            // Restore sort state if available
            if (!string.IsNullOrEmpty(config.LastSortColumn))
            {
                var sortColumn = dgvAttributes.Columns[config.LastSortColumn];
                if (sortColumn != null && dgvAttributes.DataSource != null)
                {
                    dgvAttributes.Sort(sortColumn,
                        config.LastSortAscending ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending);
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

        private void rdoSelectedSolution_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSelectedSolution.Checked)
            {
                lblSolution.Enabled = true;
                cboSolutions.Enabled = true;
            }
        }

        private void rdoAllEntities_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllEntities.Checked)
            {
                lblSolution.Enabled = false;
                cboSolutions.Enabled = false;
                cboSolutions.SelectedIndex = -1; // Clear selection
            }
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
                    RefreshGrid();

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
                    var query = new QueryExpression("solutioncomponent")
                    {
                        ColumnSet = new ColumnSet("objectid", "componenttype"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("solutionid", ConditionOperator.Equal, solution.Id),
                                new ConditionExpression("componenttype", ConditionOperator.Equal, 1) // Entity
                            }
                        }
                    };

                    var solutionComponents = Service.RetrieveMultiple(query);
                    var entityIds = solutionComponents.Entities.Select(e => e.GetAttributeValue<Guid>("objectid")).ToList();

                    var attributes = new List<AttributeMetadataInfo>();
                    foreach (var entityId in entityIds)
                    {
                        try
                        {
                            var entityRequest = new RetrieveEntityRequest
                            {
                                EntityFilters = EntityFilters.Attributes,
                                LogicalName = GetEntityLogicalName(entityId),
                                RetrieveAsIfPublished = true
                            };

                            var entityResponse = (RetrieveEntityResponse)Service.Execute(entityRequest);
                            var entity = entityResponse.EntityMetadata;

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
                            Console.WriteLine($"Error processing entity {entityId}: {ex.Message}");
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
                    RefreshGrid();

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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var searchText = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredAttributes = new List<AttributeMetadataInfo>(_allAttributes);
            }
            else
            {
                _filteredAttributes = _allAttributes.Where(a =>
                    (a.TableLogicalName?.ToLower().Contains(searchText) ?? false) ||
                    (a.TableDisplayName?.ToLower().Contains(searchText) ?? false) ||
                    (a.AttributeLogicalName?.ToLower().Contains(searchText) ?? false) ||
                    (a.AttributeDisplayName?.ToLower().Contains(searchText) ?? false) ||
                    (a.AttributeType?.ToLower().Contains(searchText) ?? false) ||
                    (a.Description?.ToLower().Contains(searchText) ?? false)
                ).ToList();
            }

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvAttributes.DataSource = null;
            dgvAttributes.DataSource = _filteredAttributes;
            lblAttributeCount.Text = $"Total Attributes: {_allAttributes.Count}";
            lblFilterStatus.Text = $"Showing {_filteredAttributes.Count} of {_allAttributes.Count} attributes";
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
    }
}
