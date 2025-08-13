using System;
using System.Drawing;
using System.Windows.Forms;
using McTools.Xrm.Connection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace AttributeExporterXrmToolBoxPlugin
{
    public class AttributeExporterPlugin : PluginBase, IGitHubPlugin, IHelpPlugin
    {
        public override string Name => "Attribute Metadata Exporter";
        public override string Description => "Export attribute metadata from Dynamics 365/Power Platform to CSV";
        public override string Company => "Your Company";
        public override string Version => "1.0.0";
        public override string HelpUrl => "https://github.com/yourusername/AttributeExporterXrmToolBoxPlugin";
        public override string RepositoryName => "AttributeExporterXrmToolBoxPlugin";
        public override string RepositoryUrl => "https://github.com/yourusername/AttributeExporterXrmToolBoxPlugin";

        public override Image Icon => Properties.Resources.icon;

        public override void Initialize()
        {
            // Plugin initialization
        }

        public override void Close()
        {
            // Plugin cleanup
        }

        protected override void ExecuteTool()
        {
            if (ConnectionDetail?.OrganizationServiceProxy == null)
            {
                MessageBox.Show("Please connect to an organization first!", "Connection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var form = new AttributeExporterForm(ConnectionDetail);
            form.ShowDialog();
        }
    }
}
