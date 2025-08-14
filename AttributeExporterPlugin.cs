using System;
using System.Drawing;
using System.Windows.Forms;
using McTools.Xrm.Connection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace AttributeExporterXrmToolBoxPlugin
{
    public class AttributeExporterPlugin : PluginBase, IGitHubPlugin
    {
        public string RepositoryName => "AttributeExporterXrmToolBoxPlugin";
        public string UserName => "your-github-username"; // Please change this to your GitHub username

        public override IXrmToolBoxPluginControl GetControl()
        {
            return new AttributeExporterControl();
        }
    }
}
