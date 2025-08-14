using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using McTools.Xrm.Connection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace AttributeExporterXrmToolBoxPlugin
{
    [Export(typeof(IXrmToolBoxPlugin))]
    [ExportMetadata("Name", "Attribute Metadata Exporter")]
    [ExportMetadata("Description", "Export attribute metadata from Dynamics 365/Power Platform to CSV")]
    [ExportMetadata("SmallImageBase64", "")] // Replace with a base64 string of a 16x16 icon
    [ExportMetadata("BigImageBase64", "")]   // Replace with a base64 string of a 32x32 icon
    [ExportMetadata("BackgroundColor", "White")]
    [ExportMetadata("PrimaryFontColor", "Black")]
    [ExportMetadata("SecondaryFontColor", "Gray")]
    public class AttributeExporterPlugin : PluginBase, IGitHubPlugin
    {
        public string RepositoryName => "AttributeExporterXrmToolBoxPlugin";
        public string UserName => "your-github-username"; 

        public override IXrmToolBoxPluginControl GetControl()
        {
            return new AttributeExporterControl();
        }
    }
}
