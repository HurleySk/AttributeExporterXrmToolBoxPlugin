using System;

namespace AttributeExporterXrmToolBoxPlugin.Models
{
    /// <summary>
    /// Defines a column's configuration for the DataGridView
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Property name in AttributeMetadataInfo
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display text for column header
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Column category for grouping in configuration dialog
        /// </summary>
        public ColumnCategory Category { get; set; }

        /// <summary>
        /// Whether this column is currently visible
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Display order (lower numbers appear first)
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Column width in pixels (null = auto-size)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Tooltip description for this column
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Categories for organizing columns in configuration dialog
    /// </summary>
    public enum ColumnCategory
    {
        Core,           // Table/Attribute names, Type, Required
        Identity,       // MetadataId, SchemaName, IsPrimaryId/Name
        Metadata,       // IsManaged, IsCustom, IntroducedVersion
        Behavior,       // IsValidFor*, IsLogical, AttributeOf
        Security,       // IsSecured, CanBeSecuredFor*
        Audit,          // IsAuditEnabled
        TypeSpecific,   // TypeSpecificDetails, MaxLength, Description
        Advanced        // All other properties
    }
}
