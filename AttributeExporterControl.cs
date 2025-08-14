using AttributeExporterXrmToolBoxPlugin.Models;
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
        private List<Models.SolutionInfo> _solutions;

        public AttributeExporterControl()
        {
            InitializeComponent();
            _allAttributes = new List<AttributeMetadataInfo>();
            _solutions = new List<Models.SolutionInfo>();
            
            // Note: Service is not available in the constructor.
            // Use OnConnectionUpdated event to load solutions.
            SetupDataGridView();
        }

        private void AttributeExporterControl_Load(object sender, EventArgs e)
        {
            // The OnConnectionUpdated event is a better place for this
            // but this is a fallback for the initial load.
            if (Service != null)
            {
                LoadSolutions();
            }
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (Service != null)
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

        private void SetupDataGridView()
        {
            dgvAttributes.AutoGenerateColumns = false;
            dgvAttributes.AllowUserToAddRows = false;
            dgvAttributes.AllowUserToDeleteRows = false;
            dgvAttributes.ReadOnly = true;
            dgvAttributes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttributes.MultiSelect = true;

            // Add columns
            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TableLogicalName",
                HeaderText = "Table Logical Name",
                DataPropertyName = "TableLogicalName",
                Width = 150
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TableDisplayName",
                HeaderText = "Table Display Name",
                DataPropertyName = "TableDisplayName",
                Width = 150
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AttributeLogicalName",
                HeaderText = "Attribute Logical Name",
                DataPropertyName = "AttributeLogicalName",
                Width = 150
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AttributeDisplayName",
                HeaderText = "Attribute Display Name",
                DataPropertyName = "AttributeDisplayName",
                Width = 150
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AttributeType",
                HeaderText = "Attribute Type",
                DataPropertyName = "AttributeType",
                Width = 100
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Required",
                HeaderText = "Required",
                DataPropertyName = "Required",
                Width = 80
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaxLength",
                HeaderText = "Max Length",
                DataPropertyName = "MaxLength",
                Width = 100
            });

            dgvAttributes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });
        }

        private void btnLoadAttributes_Click(object sender, EventArgs e)
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
                                    attributes.Add(new AttributeMetadataInfo
                                    {
                                        TableLogicalName = entity.LogicalName,
                                        TableDisplayName = entity.DisplayName?.UserLocalizedLabel?.Label ?? entity.LogicalName,
                                        AttributeLogicalName = attribute.LogicalName,
                                        AttributeDisplayName = attribute.DisplayName?.UserLocalizedLabel?.Label ?? attribute.LogicalName,
                                        AttributeType = attribute.AttributeType?.ToString() ?? "Unknown",
                                        Required = attribute.RequiredLevel?.Value == AttributeRequiredLevel.ApplicationRequired,
                                        MaxLength = GetMaxLength(attribute),
                                        Description = attribute.Description?.UserLocalizedLabel?.Label ?? ""
                                    });
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
                    dgvAttributes.DataSource = null; // Unbind first
                    dgvAttributes.DataSource = _allAttributes;
                    lblAttributeCount.Text = $"Total Attributes: {_allAttributes.Count}";
                    btnExport.Enabled = _allAttributes.Count > 0;

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

        private void ExportToCsv(string filePath)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Exporting to CSV...",
                Work = (worker, args) =>
                {
                    using (var writer = new StreamWriter(filePath))
                    using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(_allAttributes);
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
                    MessageBox.Show($"Successfully exported {_allAttributes.Count} attributes to:\n{args.Result}",
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
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
