namespace AttributeExporterXrmToolBoxPlugin.Models
{
    /// <summary>
    /// Predefined column visibility presets
    /// </summary>
    public enum ColumnPreset
    {
        /// <summary>
        /// Basic set: 8 columns - Table, Attribute, Type, Required, MaxLength, TypeSpecificDetails, Description
        /// </summary>
        Basic,

        /// <summary>
        /// Standard set: Basic + 8 more - SchemaName, IsCustom, IsPrimaryId, IsValidFor*, IntroducedVersion
        /// </summary>
        Standard,

        /// <summary>
        /// Advanced set: Standard + 12 more - Security, Audit, IsManaged, etc.
        /// </summary>
        Advanced,

        /// <summary>
        /// Full set: All 40+ available columns
        /// </summary>
        Full,

        /// <summary>
        /// User-customized column selection
        /// </summary>
        Custom
    }
}
