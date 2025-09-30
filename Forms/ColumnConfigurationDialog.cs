using AttributeExporterXrmToolBoxPlugin.Models;
using AttributeExporterXrmToolBoxPlugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AttributeExporterXrmToolBoxPlugin.Forms
{
    public partial class ColumnConfigurationDialog : Form
    {
        private ColumnConfiguration _configuration;
        private bool _isUpdatingFromPreset = false;

        public ColumnConfiguration Configuration => _configuration;

        public ColumnConfigurationDialog(ColumnConfiguration currentConfig)
        {
            InitializeComponent();
            _configuration = new ColumnConfiguration
            {
                SelectedPreset = currentConfig.SelectedPreset,
                Columns = currentConfig.Columns.Select(c => new ColumnDefinition
                {
                    Name = c.Name,
                    DisplayName = c.DisplayName,
                    Category = c.Category,
                    IsVisible = c.IsVisible,
                    DisplayOrder = c.DisplayOrder,
                    Width = c.Width,
                    Description = c.Description
                }).ToList(),
                LastSortColumn = currentConfig.LastSortColumn,
                LastSortAscending = currentConfig.LastSortAscending
            };
        }

        private void ColumnConfigurationDialog_Load(object sender, EventArgs e)
        {
            // Set up preset radio buttons
            switch (_configuration.SelectedPreset)
            {
                case ColumnPreset.Basic:
                    rdoBasic.Checked = true;
                    break;
                case ColumnPreset.Standard:
                    rdoStandard.Checked = true;
                    break;
                case ColumnPreset.Advanced:
                    rdoAdvanced.Checked = true;
                    break;
                case ColumnPreset.Full:
                    rdoFull.Checked = true;
                    break;
                case ColumnPreset.Custom:
                    rdoCustom.Checked = true;
                    break;
            }

            LoadColumnList();
        }

        private void LoadColumnList()
        {
            lstColumns.Items.Clear();

            var groupedColumns = _configuration.Columns
                .OrderBy(c => c.Category)
                .ThenBy(c => c.DisplayOrder)
                .ToList();

            ColumnCategory? lastCategory = null;

            foreach (var column in groupedColumns)
            {
                // Add category header if changed
                if (lastCategory != column.Category)
                {
                    lstColumns.Items.Add($"--- {column.Category} ---", CheckState.Indeterminate);
                    lastCategory = column.Category;
                }

                // Add column with checkbox
                lstColumns.Items.Add(column, column.IsVisible);
            }
        }

        private void rdoPreset_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingFromPreset)
                return;

            RadioButton radio = sender as RadioButton;
            if (radio == null || !radio.Checked)
                return;

            _isUpdatingFromPreset = true;

            ColumnPreset selectedPreset = ColumnPreset.Custom;

            if (radio == rdoBasic)
                selectedPreset = ColumnPreset.Basic;
            else if (radio == rdoStandard)
                selectedPreset = ColumnPreset.Standard;
            else if (radio == rdoAdvanced)
                selectedPreset = ColumnPreset.Advanced;
            else if (radio == rdoFull)
                selectedPreset = ColumnPreset.Full;
            else if (radio == rdoCustom)
                selectedPreset = ColumnPreset.Custom;

            if (selectedPreset != ColumnPreset.Custom)
            {
                _configuration.SelectedPreset = selectedPreset;
                var presetColumns = ColumnConfigurationService.GetPresetColumns(selectedPreset);

                // Update visibility based on preset
                foreach (var col in _configuration.Columns)
                {
                    var presetCol = presetColumns.FirstOrDefault(p => p.Name == col.Name);
                    if (presetCol != null)
                        col.IsVisible = presetCol.IsVisible;
                }

                LoadColumnList();
            }

            _isUpdatingFromPreset = false;
        }

        private void lstColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Prevent checking category headers
            var item = lstColumns.Items[e.Index];
            if (item is string)
            {
                e.NewValue = CheckState.Indeterminate;
                return;
            }

            // Update column visibility
            if (item is ColumnDefinition colDef)
            {
                this.BeginInvoke(new Action(() =>
                {
                    colDef.IsVisible = lstColumns.GetItemChecked(e.Index);
                    _configuration.SelectedPreset = ColumnPreset.Custom;
                    rdoCustom.Checked = true;
                }));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
