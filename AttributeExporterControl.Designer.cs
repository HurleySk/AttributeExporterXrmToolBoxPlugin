namespace AttributeExporterXrmToolBoxPlugin
{
    partial class AttributeExporterControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSolution = new System.Windows.Forms.GroupBox();
            this.btnLoadAttributes = new System.Windows.Forms.Button();
            this.rdoAllEntities = new System.Windows.Forms.RadioButton();
            this.rdoSelectedSolution = new System.Windows.Forms.RadioButton();
            this.lblScope = new System.Windows.Forms.Label();
            this.lblSolution = new System.Windows.Forms.Label();
            this.cboSolutions = new System.Windows.Forms.ComboBox();
            this.grpAttributes = new System.Windows.Forms.GroupBox();
            this.chkShowAdvancedFilters = new System.Windows.Forms.CheckBox();
            this.pnlAdvancedFilters = new System.Windows.Forms.Panel();
            this.lblFilterTable = new System.Windows.Forms.Label();
            this.txtFilterTable = new System.Windows.Forms.TextBox();
            this.lblFilterAttribute = new System.Windows.Forms.Label();
            this.txtFilterAttribute = new System.Windows.Forms.TextBox();
            this.lblFilterSchema = new System.Windows.Forms.Label();
            this.txtFilterSchema = new System.Windows.Forms.TextBox();
            this.lblFilterType = new System.Windows.Forms.Label();
            this.cboFilterType = new System.Windows.Forms.ComboBox();
            this.lblFilterRequired = new System.Windows.Forms.Label();
            this.cboFilterRequired = new System.Windows.Forms.ComboBox();
            this.lblFilterCustom = new System.Windows.Forms.Label();
            this.cboFilterCustom = new System.Windows.Forms.ComboBox();
            this.lblFilterPrimaryId = new System.Windows.Forms.Label();
            this.cboFilterPrimaryId = new System.Windows.Forms.ComboBox();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblAttributeCount = new System.Windows.Forms.Label();
            this.dgvAttributes = new System.Windows.Forms.DataGridView();
            this.lblConnectionMessage = new System.Windows.Forms.Label();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnColumns = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.grpSolution.SuspendLayout();
            this.grpAttributes.SuspendLayout();
            this.pnlAdvancedFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).BeginInit();
            this.grpActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSolution
            //
            this.grpSolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSolution.Controls.Add(this.btnLoadAttributes);
            this.grpSolution.Controls.Add(this.rdoAllEntities);
            this.grpSolution.Controls.Add(this.rdoSelectedSolution);
            this.grpSolution.Controls.Add(this.lblScope);
            this.grpSolution.Controls.Add(this.lblSolution);
            this.grpSolution.Controls.Add(this.cboSolutions);
            this.grpSolution.Location = new System.Drawing.Point(12, 12);
            this.grpSolution.Name = "grpSolution";
            this.grpSolution.Size = new System.Drawing.Size(960, 100);
            this.grpSolution.TabIndex = 0;
            this.grpSolution.TabStop = false;
            this.grpSolution.Text = "Data Source";
            // 
            // btnLoadAttributes
            //
            this.btnLoadAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadAttributes.Location = new System.Drawing.Point(740, 60);
            this.btnLoadAttributes.Name = "btnLoadAttributes";
            this.btnLoadAttributes.Size = new System.Drawing.Size(200, 25);
            this.btnLoadAttributes.TabIndex = 4;
            this.btnLoadAttributes.Text = "Load Attributes";
            this.btnLoadAttributes.UseVisualStyleBackColor = true;
            this.btnLoadAttributes.Click += new System.EventHandler(this.btnLoadAttributes_Click);
            //
            // rdoAllEntities
            //
            this.rdoAllEntities.AutoSize = true;
            this.rdoAllEntities.Location = new System.Drawing.Point(240, 25);
            this.rdoAllEntities.Name = "rdoAllEntities";
            this.rdoAllEntities.Size = new System.Drawing.Size(80, 17);
            this.rdoAllEntities.TabIndex = 2;
            this.rdoAllEntities.TabStop = true;
            this.rdoAllEntities.Text = "All Entities";
            this.rdoAllEntities.UseVisualStyleBackColor = true;
            this.rdoAllEntities.CheckedChanged += new System.EventHandler(this.rdoAllEntities_CheckedChanged);
            //
            // rdoSelectedSolution
            //
            this.rdoSelectedSolution.AutoSize = true;
            this.rdoSelectedSolution.Checked = true;
            this.rdoSelectedSolution.Location = new System.Drawing.Point(80, 25);
            this.rdoSelectedSolution.Name = "rdoSelectedSolution";
            this.rdoSelectedSolution.Size = new System.Drawing.Size(115, 17);
            this.rdoSelectedSolution.TabIndex = 1;
            this.rdoSelectedSolution.TabStop = true;
            this.rdoSelectedSolution.Text = "Selected Solution";
            this.rdoSelectedSolution.UseVisualStyleBackColor = true;
            this.rdoSelectedSolution.CheckedChanged += new System.EventHandler(this.rdoSelectedSolution_CheckedChanged);
            //
            // lblScope
            //
            this.lblScope.AutoSize = true;
            this.lblScope.Location = new System.Drawing.Point(20, 27);
            this.lblScope.Name = "lblScope";
            this.lblScope.Size = new System.Drawing.Size(42, 13);
            this.lblScope.TabIndex = 0;
            this.lblScope.Text = "Scope:";
            //
            // lblSolution
            //
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(20, 60);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(51, 13);
            this.lblSolution.TabIndex = 3;
            this.lblSolution.Text = "Solution:";
            //
            // cboSolutions
            //
            this.cboSolutions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSolutions.FormattingEnabled = true;
            this.cboSolutions.Location = new System.Drawing.Point(80, 57);
            this.cboSolutions.Name = "cboSolutions";
            this.cboSolutions.Size = new System.Drawing.Size(640, 21);
            this.cboSolutions.TabIndex = 4;
            // 
            // grpAttributes
            //
            this.grpAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAttributes.Controls.Add(this.chkShowAdvancedFilters);
            this.grpAttributes.Controls.Add(this.pnlAdvancedFilters);
            this.grpAttributes.Controls.Add(this.lblFilterStatus);
            this.grpAttributes.Controls.Add(this.txtSearch);
            this.grpAttributes.Controls.Add(this.lblSearch);
            this.grpAttributes.Controls.Add(this.lblAttributeCount);
            this.grpAttributes.Controls.Add(this.dgvAttributes);
            this.grpAttributes.Controls.Add(this.lblConnectionMessage);
            this.grpAttributes.Location = new System.Drawing.Point(12, 118);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(960, 380);
            this.grpAttributes.TabIndex = 1;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Attributes";
            //
            // chkShowAdvancedFilters
            //
            this.chkShowAdvancedFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowAdvancedFilters.AutoSize = true;
            this.chkShowAdvancedFilters.Location = new System.Drawing.Point(780, 22);
            this.chkShowAdvancedFilters.Name = "chkShowAdvancedFilters";
            this.chkShowAdvancedFilters.Size = new System.Drawing.Size(160, 17);
            this.chkShowAdvancedFilters.TabIndex = 6;
            this.chkShowAdvancedFilters.Text = "Show Advanced Filters";
            this.chkShowAdvancedFilters.UseVisualStyleBackColor = true;
            this.chkShowAdvancedFilters.CheckedChanged += new System.EventHandler(this.chkShowAdvancedFilters_CheckedChanged);
            //
            // pnlAdvancedFilters
            //
            this.pnlAdvancedFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAdvancedFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterTable);
            this.pnlAdvancedFilters.Controls.Add(this.txtFilterTable);
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterAttribute);
            this.pnlAdvancedFilters.Controls.Add(this.txtFilterAttribute);
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterType);
            this.pnlAdvancedFilters.Controls.Add(this.cboFilterType);
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterRequired);
            this.pnlAdvancedFilters.Controls.Add(this.cboFilterRequired);
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterCustom);
            this.pnlAdvancedFilters.Controls.Add(this.cboFilterCustom);
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterPrimaryId);
            this.pnlAdvancedFilters.Controls.Add(this.cboFilterPrimaryId);
            this.pnlAdvancedFilters.Controls.Add(this.lblFilterSchema);
            this.pnlAdvancedFilters.Controls.Add(this.txtFilterSchema);
            this.pnlAdvancedFilters.Controls.Add(this.btnClearFilters);
            this.pnlAdvancedFilters.Location = new System.Drawing.Point(20, 45);
            this.pnlAdvancedFilters.Name = "pnlAdvancedFilters";
            this.pnlAdvancedFilters.Size = new System.Drawing.Size(920, 65);
            this.pnlAdvancedFilters.TabIndex = 7;
            this.pnlAdvancedFilters.Visible = false;
            //
            // lblFilterTable
            //
            this.lblFilterTable.AutoSize = true;
            this.lblFilterTable.Location = new System.Drawing.Point(5, 8);
            this.lblFilterTable.Name = "lblFilterTable";
            this.lblFilterTable.Size = new System.Drawing.Size(37, 13);
            this.lblFilterTable.TabIndex = 0;
            this.lblFilterTable.Text = "Table:";
            //
            // txtFilterTable
            //
            this.txtFilterTable.Location = new System.Drawing.Point(50, 5);
            this.txtFilterTable.Name = "txtFilterTable";
            this.txtFilterTable.Size = new System.Drawing.Size(140, 20);
            this.txtFilterTable.TabIndex = 1;
            this.txtFilterTable.TextChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // lblFilterAttribute
            //
            this.lblFilterAttribute.AutoSize = true;
            this.lblFilterAttribute.Location = new System.Drawing.Point(200, 8);
            this.lblFilterAttribute.Name = "lblFilterAttribute";
            this.lblFilterAttribute.Size = new System.Drawing.Size(54, 13);
            this.lblFilterAttribute.TabIndex = 2;
            this.lblFilterAttribute.Text = "Attribute:";
            //
            // txtFilterAttribute
            //
            this.txtFilterAttribute.Location = new System.Drawing.Point(260, 5);
            this.txtFilterAttribute.Name = "txtFilterAttribute";
            this.txtFilterAttribute.Size = new System.Drawing.Size(140, 20);
            this.txtFilterAttribute.TabIndex = 3;
            this.txtFilterAttribute.TextChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // lblFilterType
            //
            this.lblFilterType.AutoSize = true;
            this.lblFilterType.Location = new System.Drawing.Point(410, 8);
            this.lblFilterType.Name = "lblFilterType";
            this.lblFilterType.Size = new System.Drawing.Size(34, 13);
            this.lblFilterType.TabIndex = 4;
            this.lblFilterType.Text = "Type:";
            //
            // cboFilterType
            //
            this.cboFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterType.FormattingEnabled = true;
            this.cboFilterType.Location = new System.Drawing.Point(450, 5);
            this.cboFilterType.Name = "cboFilterType";
            this.cboFilterType.Size = new System.Drawing.Size(120, 21);
            this.cboFilterType.TabIndex = 5;
            this.cboFilterType.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // lblFilterRequired
            //
            this.lblFilterRequired.AutoSize = true;
            this.lblFilterRequired.Location = new System.Drawing.Point(5, 38);
            this.lblFilterRequired.Name = "lblFilterRequired";
            this.lblFilterRequired.Size = new System.Drawing.Size(56, 13);
            this.lblFilterRequired.TabIndex = 6;
            this.lblFilterRequired.Text = "Required:";
            //
            // cboFilterRequired
            //
            this.cboFilterRequired.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterRequired.FormattingEnabled = true;
            this.cboFilterRequired.Items.AddRange(new object[] { "All", "Yes", "No" });
            this.cboFilterRequired.Location = new System.Drawing.Point(70, 35);
            this.cboFilterRequired.Name = "cboFilterRequired";
            this.cboFilterRequired.Size = new System.Drawing.Size(80, 21);
            this.cboFilterRequired.TabIndex = 7;
            this.cboFilterRequired.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // lblFilterCustom
            //
            this.lblFilterCustom.AutoSize = true;
            this.lblFilterCustom.Location = new System.Drawing.Point(160, 38);
            this.lblFilterCustom.Name = "lblFilterCustom";
            this.lblFilterCustom.Size = new System.Drawing.Size(48, 13);
            this.lblFilterCustom.TabIndex = 8;
            this.lblFilterCustom.Text = "Custom:";
            //
            // cboFilterCustom
            //
            this.cboFilterCustom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterCustom.FormattingEnabled = true;
            this.cboFilterCustom.Items.AddRange(new object[] { "All", "Yes", "No" });
            this.cboFilterCustom.Location = new System.Drawing.Point(215, 35);
            this.cboFilterCustom.Name = "cboFilterCustom";
            this.cboFilterCustom.Size = new System.Drawing.Size(80, 21);
            this.cboFilterCustom.TabIndex = 9;
            this.cboFilterCustom.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // lblFilterPrimaryId
            //
            this.lblFilterPrimaryId.AutoSize = true;
            this.lblFilterPrimaryId.Location = new System.Drawing.Point(305, 38);
            this.lblFilterPrimaryId.Name = "lblFilterPrimaryId";
            this.lblFilterPrimaryId.Size = new System.Drawing.Size(64, 13);
            this.lblFilterPrimaryId.TabIndex = 10;
            this.lblFilterPrimaryId.Text = "Primary ID:";
            //
            // cboFilterPrimaryId
            //
            this.cboFilterPrimaryId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterPrimaryId.FormattingEnabled = true;
            this.cboFilterPrimaryId.Items.AddRange(new object[] { "All", "Yes", "No" });
            this.cboFilterPrimaryId.Location = new System.Drawing.Point(375, 35);
            this.cboFilterPrimaryId.Name = "cboFilterPrimaryId";
            this.cboFilterPrimaryId.Size = new System.Drawing.Size(80, 21);
            this.cboFilterPrimaryId.TabIndex = 11;
            this.cboFilterPrimaryId.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // lblFilterSchema
            //
            this.lblFilterSchema.AutoSize = true;
            this.lblFilterSchema.Location = new System.Drawing.Point(465, 38);
            this.lblFilterSchema.Name = "lblFilterSchema";
            this.lblFilterSchema.Size = new System.Drawing.Size(49, 13);
            this.lblFilterSchema.TabIndex = 12;
            this.lblFilterSchema.Text = "Schema:";
            //
            // txtFilterSchema
            //
            this.txtFilterSchema.Location = new System.Drawing.Point(520, 35);
            this.txtFilterSchema.Name = "txtFilterSchema";
            this.txtFilterSchema.Size = new System.Drawing.Size(140, 20);
            this.txtFilterSchema.TabIndex = 13;
            this.txtFilterSchema.TextChanged += new System.EventHandler(this.FilterControl_Changed);
            //
            // btnClearFilters
            //
            this.btnClearFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilters.Location = new System.Drawing.Point(825, 20);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(85, 25);
            this.btnClearFilters.TabIndex = 14;
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.btnClearFilters_Click);
            //
            // lblFilterStatus
            //
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(20, 50);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(110, 13);
            this.lblFilterStatus.TabIndex = 4;
            this.lblFilterStatus.Text = "Showing 0 of 0 attributes";
            //
            // txtSearch
            //
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(80, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(860, 20);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 23);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Search:";
            //
            // lblAttributeCount
            //
            this.lblAttributeCount.AutoSize = true;
            this.lblAttributeCount.Location = new System.Drawing.Point(700, 50);
            this.lblAttributeCount.Name = "lblAttributeCount";
            this.lblAttributeCount.Size = new System.Drawing.Size(89, 13);
            this.lblAttributeCount.TabIndex = 1;
            this.lblAttributeCount.Text = "Total Attributes: 0";
            //
            // dgvAttributes
            //
            this.dgvAttributes.AllowUserToAddRows = false;
            this.dgvAttributes.AllowUserToDeleteRows = false;
            this.dgvAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttributes.Location = new System.Drawing.Point(20, 70);
            this.dgvAttributes.Name = "dgvAttributes";
            this.dgvAttributes.ReadOnly = true;
            this.dgvAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttributes.Size = new System.Drawing.Size(920, 290);
            this.dgvAttributes.TabIndex = 0;
            //
            // lblConnectionMessage
            //
            this.lblConnectionMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectionMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionMessage.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblConnectionMessage.Location = new System.Drawing.Point(300, 23);
            this.lblConnectionMessage.Name = "lblConnectionMessage";
            this.lblConnectionMessage.Size = new System.Drawing.Size(400, 20);
            this.lblConnectionMessage.TabIndex = 5;
            this.lblConnectionMessage.Text = "⚠ Not connected - Click Load Attributes to connect";
            this.lblConnectionMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblConnectionMessage.Visible = false;
            //
            // grpActions
            // 
            this.grpActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpActions.Controls.Add(this.btnClose);
            this.grpActions.Controls.Add(this.btnColumns);
            this.grpActions.Controls.Add(this.btnExport);
            this.grpActions.Location = new System.Drawing.Point(12, 504);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(960, 60);
            this.grpActions.TabIndex = 2;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(850, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // btnColumns
            //
            this.btnColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColumns.Location = new System.Drawing.Point(630, 20);
            this.btnColumns.Name = "btnColumns";
            this.btnColumns.Size = new System.Drawing.Size(100, 30);
            this.btnColumns.TabIndex = 2;
            this.btnColumns.Text = "Columns...";
            this.btnColumns.UseVisualStyleBackColor = true;
            this.btnColumns.Click += new System.EventHandler(this.btnColumns_Click);
            //
            // btnExport
            //
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(740, 20);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 30);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Export to CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // AttributeExporterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.grpAttributes);
            this.Controls.Add(this.grpSolution);
            this.Name = "AttributeExporterControl";
            this.Size = new System.Drawing.Size(984, 576);
            this.Load += new System.EventHandler(this.AttributeExporterControl_Load);
            this.grpSolution.ResumeLayout(false);
            this.grpSolution.PerformLayout();
            this.pnlAdvancedFilters.ResumeLayout(false);
            this.pnlAdvancedFilters.PerformLayout();
            this.grpAttributes.ResumeLayout(false);
            this.grpAttributes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).EndInit();
            this.grpActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSolution;
        private System.Windows.Forms.Button btnLoadAttributes;
        private System.Windows.Forms.RadioButton rdoAllEntities;
        private System.Windows.Forms.RadioButton rdoSelectedSolution;
        private System.Windows.Forms.Label lblScope;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.ComboBox cboSolutions;
        private System.Windows.Forms.GroupBox grpAttributes;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblAttributeCount;
        private System.Windows.Forms.DataGridView dgvAttributes;
        private System.Windows.Forms.Label lblConnectionMessage;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnColumns;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkShowAdvancedFilters;
        private System.Windows.Forms.Panel pnlAdvancedFilters;
        private System.Windows.Forms.Label lblFilterTable;
        private System.Windows.Forms.TextBox txtFilterTable;
        private System.Windows.Forms.Label lblFilterAttribute;
        private System.Windows.Forms.TextBox txtFilterAttribute;
        private System.Windows.Forms.Label lblFilterSchema;
        private System.Windows.Forms.TextBox txtFilterSchema;
        private System.Windows.Forms.Label lblFilterType;
        private System.Windows.Forms.ComboBox cboFilterType;
        private System.Windows.Forms.Label lblFilterRequired;
        private System.Windows.Forms.ComboBox cboFilterRequired;
        private System.Windows.Forms.Label lblFilterCustom;
        private System.Windows.Forms.ComboBox cboFilterCustom;
        private System.Windows.Forms.Label lblFilterPrimaryId;
        private System.Windows.Forms.ComboBox cboFilterPrimaryId;
        private System.Windows.Forms.Button btnClearFilters;
    }
}
