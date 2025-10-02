using System;

namespace AttributeExporterXrmToolBoxPlugin.Models
{
    /// <summary>
    /// Comprehensive attribute metadata information from Dataverse
    /// </summary>
    public class AttributeMetadataInfo
    {
        // === Core Identification ===
        public string TableLogicalName { get; set; }
        public string TableDisplayName { get; set; }
        public string AttributeLogicalName { get; set; }
        public string AttributeDisplayName { get; set; }
        public string SchemaName { get; set; }
        public string AttributeType { get; set; }
        public string AttributeTypeName { get; set; }

        // === Metadata Properties ===
        public string MetadataId { get; set; }
        public string IntroducedVersion { get; set; }
        public bool? IsManaged { get; set; }
        public bool? IsCustomAttribute { get; set; }

        // === Field Characteristics ===
        public bool Required { get; set; }
        public string RequiredLevel { get; set; }
        public int? MaxLength { get; set; }
        public string Description { get; set; }

        // === Identity Flags ===
        public bool? IsPrimaryId { get; set; }
        public bool? IsPrimaryName { get; set; }
        public bool? IsLogical { get; set; }
        public string AttributeOf { get; set; }
        public bool? IsIntersectEntity { get; set; }

        // === Validity Flags ===
        public bool? IsValidForCreate { get; set; }
        public bool? IsValidForRead { get; set; }
        public bool? IsValidForUpdate { get; set; }

        // === Security ===
        public bool? IsSecured { get; set; }
        public bool? CanBeSecuredForCreate { get; set; }
        public bool? CanBeSecuredForRead { get; set; }
        public bool? CanBeSecuredForUpdate { get; set; }

        // === Audit ===
        public bool? IsAuditEnabled { get; set; }

        // === Type-Specific Details (Smart Column) ===
        /// <summary>
        /// Intelligently formatted string containing type-specific metadata
        /// (e.g., "Format: Email, MaxLen: 100" for strings, "Targets: account, contact" for lookups)
        /// </summary>
        public string TypeSpecificDetails { get; set; }
    }
}
