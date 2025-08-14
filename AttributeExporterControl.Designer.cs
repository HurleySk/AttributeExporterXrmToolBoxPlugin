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
            this.lblSolution = new System.Windows.Forms.Label();
            this.cboSolutions = new System.Windows.Forms.ComboBox();
            this.grpAttributes = new System.Windows.Forms.GroupBox();
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
            this.grpSolution.Controls.Add(this.lblSolution);
            this.grpSolution.Controls.Add(this.cboSolutions);
            this.grpSolution.Location = new System.Drawing.Point(12, 12);
            this.grpSolution.Name = "grpSolution";
            this.grpSolution.Size = new System.Drawing.Size(960, 80);
            this.grpSolution.TabIndex = 0;
            this.grpSolution.TabStop = false;
            this.grpSolution.Text = "Solution Selection";
            // 
            // btnLoadAttributes
            // 
            this.btnLoadAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadAttributes.Location = new System.Drawing.Point(740, 23);
            this.btnLoadAttributes.Name = "btnLoadAttributes";
            this.btnLoadAttributes.Size = new System.Drawing.Size(200, 25);
            this.btnLoadAttributes.TabIndex = 2;
            this.btnLoadAttributes.Text = "Load Attributes";
            this.btnLoadAttributes.UseVisualStyleBackColor = true;
            this.btnLoadAttributes.Click += new System.EventHandler(this.btnLoadAttributes_Click);
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(20, 28);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(94, 13);
            this.lblSolution.TabIndex = 1;
            this.lblSolution.Text = "Select Solution:";
            // 
            // cboSolutions
            // 
            this.cboSolutions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSolutions.FormattingEnabled = true;
            this.cboSolutions.Location = new System.Drawing.Point(120, 25);
            this.cboSolutions.Name = "cboSolutions";
            this.cboSolutions.Size = new System.Drawing.Size(600, 21);
            this.cboSolutions.TabIndex = 0;
            // 
            // grpAttributes
            // 
            this.grpAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAttributes.Controls.Add(this.lblAttributeCount);
            this.grpAttributes.Controls.Add(this.dgvAttributes);
            this.grpAttributes.Location = new System.Drawing.Point(12, 98);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(960, 400);
            this.grpAttributes.TabIndex = 1;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Attributes";
            // 
            // lblAttributeCount
            // 
            this.lblAttributeCount.AutoSize = true;
            this.lblAttributeCount.Location = new System.Drawing.Point(20, 20);
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
            this.dgvAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttributes.Location = new System.Drawing.Point(20, 40);
            this.dgvAttributes.Name = "dgvAttributes";
            this.dgvAttributes.ReadOnly = true;
            this.dgvAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttributes.Size = new System.Drawing.Size(920, 320);
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
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.ComboBox cboSolutions;
        private System.Windows.Forms.GroupBox grpAttributes;
        private System.Windows.Forms.Label lblAttributeCount;
        private System.Windows.Forms.DataGridView dgvAttributes;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
    }
}
