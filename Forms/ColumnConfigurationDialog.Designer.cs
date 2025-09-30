namespace AttributeExporterXrmToolBoxPlugin.Forms
{
    partial class ColumnConfigurationDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpPresets = new System.Windows.Forms.GroupBox();
            this.rdoCustom = new System.Windows.Forms.RadioButton();
            this.rdoFull = new System.Windows.Forms.RadioButton();
            this.rdoAdvanced = new System.Windows.Forms.RadioButton();
            this.rdoStandard = new System.Windows.Forms.RadioButton();
            this.rdoBasic = new System.Windows.Forms.RadioButton();
            this.lstColumns = new System.Windows.Forms.CheckedListBox();
            this.lblColumns = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpPresets.SuspendLayout();
            this.SuspendLayout();
            //
            // grpPresets
            //
            this.grpPresets.Controls.Add(this.rdoCustom);
            this.grpPresets.Controls.Add(this.rdoFull);
            this.grpPresets.Controls.Add(this.rdoAdvanced);
            this.grpPresets.Controls.Add(this.rdoStandard);
            this.grpPresets.Controls.Add(this.rdoBasic);
            this.grpPresets.Location = new System.Drawing.Point(12, 12);
            this.grpPresets.Name = "grpPresets";
            this.grpPresets.Size = new System.Drawing.Size(560, 60);
            this.grpPresets.TabIndex = 0;
            this.grpPresets.TabStop = false;
            this.grpPresets.Text = "Column Presets";
            //
            // rdoCustom
            //
            this.rdoCustom.AutoSize = true;
            this.rdoCustom.Location = new System.Drawing.Point(460, 25);
            this.rdoCustom.Name = "rdoCustom";
            this.rdoCustom.Size = new System.Drawing.Size(85, 17);
            this.rdoCustom.TabIndex = 4;
            this.rdoCustom.TabStop = true;
            this.rdoCustom.Text = "Custom";
            this.rdoCustom.UseVisualStyleBackColor = true;
            this.rdoCustom.CheckedChanged += new System.EventHandler(this.rdoPreset_CheckedChanged);
            //
            // rdoFull
            //
            this.rdoFull.AutoSize = true;
            this.rdoFull.Location = new System.Drawing.Point(350, 25);
            this.rdoFull.Name = "rdoFull";
            this.rdoFull.Size = new System.Drawing.Size(100, 17);
            this.rdoFull.TabIndex = 3;
            this.rdoFull.TabStop = true;
            this.rdoFull.Text = "Full (All Columns)";
            this.rdoFull.UseVisualStyleBackColor = true;
            this.rdoFull.CheckedChanged += new System.EventHandler(this.rdoPreset_CheckedChanged);
            //
            // rdoAdvanced
            //
            this.rdoAdvanced.AutoSize = true;
            this.rdoAdvanced.Location = new System.Drawing.Point(230, 25);
            this.rdoAdvanced.Name = "rdoAdvanced";
            this.rdoAdvanced.Size = new System.Drawing.Size(110, 17);
            this.rdoAdvanced.TabIndex = 2;
            this.rdoAdvanced.TabStop = true;
            this.rdoAdvanced.Text = "Advanced (28 cols)";
            this.rdoAdvanced.UseVisualStyleBackColor = true;
            this.rdoAdvanced.CheckedChanged += new System.EventHandler(this.rdoPreset_CheckedChanged);
            //
            // rdoStandard
            //
            this.rdoStandard.AutoSize = true;
            this.rdoStandard.Location = new System.Drawing.Point(120, 25);
            this.rdoStandard.Name = "rdoStandard";
            this.rdoStandard.Size = new System.Drawing.Size(105, 17);
            this.rdoStandard.TabIndex = 1;
            this.rdoStandard.TabStop = true;
            this.rdoStandard.Text = "Standard (16 cols)";
            this.rdoStandard.UseVisualStyleBackColor = true;
            this.rdoStandard.CheckedChanged += new System.EventHandler(this.rdoPreset_CheckedChanged);
            //
            // rdoBasic
            //
            this.rdoBasic.AutoSize = true;
            this.rdoBasic.Location = new System.Drawing.Point(15, 25);
            this.rdoBasic.Name = "rdoBasic";
            this.rdoBasic.Size = new System.Drawing.Size(85, 17);
            this.rdoBasic.TabIndex = 0;
            this.rdoBasic.TabStop = true;
            this.rdoBasic.Text = "Basic (8 cols)";
            this.rdoBasic.UseVisualStyleBackColor = true;
            this.rdoBasic.CheckedChanged += new System.EventHandler(this.rdoPreset_CheckedChanged);
            //
            // lstColumns
            //
            this.lstColumns.CheckOnClick = true;
            this.lstColumns.FormattingEnabled = true;
            this.lstColumns.Location = new System.Drawing.Point(12, 103);
            this.lstColumns.Name = "lstColumns";
            this.lstColumns.Size = new System.Drawing.Size(560, 364);
            this.lstColumns.TabIndex = 1;
            this.lstColumns.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstColumns_ItemCheck);
            //
            // lblColumns
            //
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(12, 82);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(203, 13);
            this.lblColumns.TabIndex = 2;
            this.lblColumns.Text = "Select Columns to Display (check to show):";
            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(416, 483);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 28);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(497, 483);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // ColumnConfigurationDialog
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 521);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.lstColumns);
            this.Controls.Add(this.grpPresets);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColumnConfigurationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Columns";
            this.Load += new System.EventHandler(this.ColumnConfigurationDialog_Load);
            this.grpPresets.ResumeLayout(false);
            this.grpPresets.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPresets;
        private System.Windows.Forms.RadioButton rdoCustom;
        private System.Windows.Forms.RadioButton rdoFull;
        private System.Windows.Forms.RadioButton rdoAdvanced;
        private System.Windows.Forms.RadioButton rdoStandard;
        private System.Windows.Forms.RadioButton rdoBasic;
        private System.Windows.Forms.CheckedListBox lstColumns;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
