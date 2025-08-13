using System;

namespace AttributeExporterXrmToolBoxPlugin.Models
{
    public class AttributeMetadataInfo
    {
        public string TableLogicalName { get; set; }
        public string TableDisplayName { get; set; }
        public string AttributeLogicalName { get; set; }
        public string AttributeDisplayName { get; set; }
        public string AttributeType { get; set; }
        public bool Required { get; set; }
        public int? MaxLength { get; set; }
        public string Description { get; set; }
    }
}
