namespace AttributeExporterXrmToolBoxPlugin.Models
{
    /// <summary>
    /// Represents the active filter criteria for advanced column filtering
    /// </summary>
    public class FilterCriteria
    {
        /// <summary>
        /// Filter text for table name (searches both TableLogicalName and TableDisplayName)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Filter text for attribute name (searches both AttributeLogicalName and AttributeDisplayName)
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Filter text for schema name
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Filter for attribute type (e.g., "String", "Integer", "Lookup", or "All")
        /// </summary>
        public string AttributeType { get; set; }

        /// <summary>
        /// Filter for required status: "All", "Yes", or "No"
        /// </summary>
        public string Required { get; set; }

        /// <summary>
        /// Filter for custom attribute flag: "All", "Yes", or "No"
        /// </summary>
        public string IsCustom { get; set; }

        /// <summary>
        /// Filter for primary ID flag: "All", "Yes", or "No"
        /// </summary>
        public string IsPrimaryId { get; set; }

        /// <summary>
        /// Creates a new FilterCriteria with default values
        /// </summary>
        public FilterCriteria()
        {
            TableName = string.Empty;
            AttributeName = string.Empty;
            SchemaName = string.Empty;
            AttributeType = "All";
            Required = "All";
            IsCustom = "All";
            IsPrimaryId = "All";
        }

        /// <summary>
        /// Checks if any filters are active (non-default values)
        /// </summary>
        public bool HasActiveFilters()
        {
            return !string.IsNullOrWhiteSpace(TableName) ||
                   !string.IsNullOrWhiteSpace(AttributeName) ||
                   !string.IsNullOrWhiteSpace(SchemaName) ||
                   (AttributeType != "All" && !string.IsNullOrWhiteSpace(AttributeType)) ||
                   (Required != "All" && !string.IsNullOrWhiteSpace(Required)) ||
                   (IsCustom != "All" && !string.IsNullOrWhiteSpace(IsCustom)) ||
                   (IsPrimaryId != "All" && !string.IsNullOrWhiteSpace(IsPrimaryId));
        }

        /// <summary>
        /// Resets all filters to default values
        /// </summary>
        public void Reset()
        {
            TableName = string.Empty;
            AttributeName = string.Empty;
            SchemaName = string.Empty;
            AttributeType = "All";
            Required = "All";
            IsCustom = "All";
            IsPrimaryId = "All";
        }
    }
}
