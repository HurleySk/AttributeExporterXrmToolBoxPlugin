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
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblAttributeCount = new System.Windows.Forms.Label();
            this.dgvAttributes = new System.Windows.Forms.DataGridView();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.grpSolution.SuspendLayout();
            this.grpAttributes.SuspendLayout();
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
            this.grpAttributes.Controls.Add(this.lblFilterStatus);
            this.grpAttributes.Controls.Add(this.txtSearch);
            this.grpAttributes.Controls.Add(this.lblSearch);
            this.grpAttributes.Controls.Add(this.lblAttributeCount);
            this.grpAttributes.Controls.Add(this.dgvAttributes);
            this.grpAttributes.Location = new System.Drawing.Point(12, 118);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(960, 380);
            this.grpAttributes.TabIndex = 1;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Attributes";
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
            // grpActions
            // 
            this.grpActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpActions.Controls.Add(this.btnClose);
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
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
    }
}
