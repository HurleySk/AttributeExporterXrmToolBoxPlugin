using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using CsvHelper;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace AttributeExporterXrmToolBoxPlugin
{
    public partial class AttributeExporterForm : Form
    {
        private readonly ConnectionDetail _connectionDetail;
        private readonly IOrganizationService _service;
        private List<AttributeMetadataInfo> _allAttributes;
        private List<SolutionInfo> _solutions;

        public AttributeExporterForm(ConnectionDetail connectionDetail)
        {
            InitializeComponent();
            _connectionDetail = connectionDetail;
            _service = connectionDetail.OrganizationServiceProxy;
            _allAttributes = new List<AttributeMetadataInfo>();
            _solutions = new List<SolutionInfo>();

            LoadSolutions();
            SetupDataGridView();
        }

        private void LoadSolutions()
        {
            try
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
                    },
                    OrderBy = new OrderByExpression("friendlyname", OrderType.Ascending)
                };

                var solutions = _service.RetrieveMultiple(query);
                _solutions = solutions.Entities.Select(e => new SolutionInfo
                {
                    Id = e.Id,
                    Name = e.GetAttributeValue<string>("friendlyname"),
                    UniqueName = e.GetAttributeValue<string>("uniquename"),
                    Version = e.GetAttributeValue<string>("version")
                }).ToList();

                cboSolutions.DataSource = _solutions;
                cboSolutions.DisplayMember = "Name";
                cboSolutions.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading solutions: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            var selectedSolution = (SolutionInfo)cboSolutions.SelectedItem;
            LoadAttributesForSolution(selectedSolution);
        }

        private void LoadAttributesForSolution(SolutionInfo solution)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                btnLoadAttributes.Enabled = false;
                btnExport.Enabled = false;

                // Get solution components
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

                var solutionComponents = _service.RetrieveMultiple(query);
                var entityIds = solutionComponents.Entities.Select(e => e.GetAttributeValue<Guid>("objectid")).ToList();

                _allAttributes.Clear();

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

                        var entityResponse = (RetrieveEntityResponse)_service.Execute(entityRequest);
                        var entity = entityResponse.EntityMetadata;

                        foreach (var attribute in entity.Attributes)
                        {
                            if (attribute.IsValidForRead == true)
                            {
                                _allAttributes.Add(new AttributeMetadataInfo
                                {
                                    TableLogicalName = entity.LogicalName,
                                    TableDisplayName = entity.DisplayName?.UserLocalizedLabel?.Label ?? entity.LogicalName,
                                    AttributeLogicalName = attribute.LogicalName,
                                    AttributeDisplayName = attribute.DisplayName?.UserLocalizedLabel?.Label ?? attribute.LogicalName,
                                    AttributeType = attribute.AttributeType?.ToString() ?? "Unknown",
                                    Required = attribute.RequiredLevel?.Value == AttributeRequiredLevel.Required,
                                    MaxLength = GetMaxLength(attribute),
                                    Description = attribute.Description?.UserLocalizedLabel?.Label ?? ""
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other entities
                        Console.WriteLine($"Error processing entity {entityId}: {ex.Message}");
                    }
                }

                dgvAttributes.DataSource = _allAttributes;
                lblAttributeCount.Text = $"Total Attributes: {_allAttributes.Count}";
                btnExport.Enabled = _allAttributes.Count > 0;

                MessageBox.Show($"Successfully loaded {_allAttributes.Count} attributes from solution '{solution.Name}'", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attributes: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnLoadAttributes.Enabled = true;
            }
        }

        private string GetEntityLogicalName(Guid entityId)
        {
            try
            {
                var entity = _service.Retrieve("entity", entityId, new ColumnSet("logicalname"));
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
            try
            {
                Cursor = Cursors.WaitCursor;
                btnExport.Enabled = false;

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_allAttributes);
                }

                MessageBox.Show($"Successfully exported {_allAttributes.Count} attributes to:\n{filePath}", 
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to CSV: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnExport.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
