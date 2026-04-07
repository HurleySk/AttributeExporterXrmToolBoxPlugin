using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttributeExporterXrmToolBoxPlugin.Forms
{
    /// <summary>
    /// Dialog for viewing the full content of a cell value with copy support
    /// </summary>
    public class CellValueViewerDialog : Form
    {
        public CellValueViewerDialog(string columnName, string cellValue)
        {
            Text = columnName;
            Width = 500;
            Height = 350;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            MaximizeBox = false;

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

            Controls.Add(textPanel);
            Controls.Add(buttonPanel);
            AcceptButton = btnOk;
        }
    }
}
