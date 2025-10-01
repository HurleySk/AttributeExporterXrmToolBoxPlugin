using AttributeExporterXrmToolBoxPlugin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AttributeExporterXrmToolBoxPlugin.Services
{
    /// <summary>
    /// Manages column configuration presets and persistence
    /// </summary>
    public class ColumnConfigurationService
    {
        private static readonly string ConfigDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AttributeExporter");

        private static readonly string ConfigFilePath = Path.Combine(ConfigDirectory, "config.json");

        /// <summary>
        /// All available column definitions
        /// </summary>
        public static readonly List<ColumnDefinition> AllColumns = new List<ColumnDefinition>
        {
            // Core (8)
            new ColumnDefinition { Name = "TableLogicalName", DisplayName = "Table Logical Name", Category = ColumnCategory.Core, DisplayOrder = 0, Width = 150, Description = "Logical name of the table" },
            new ColumnDefinition { Name = "TableDisplayName", DisplayName = "Table Display Name", Category = ColumnCategory.Core, DisplayOrder = 1, Width = 150, Description = "Display name of the table" },
            new ColumnDefinition { Name = "AttributeLogicalName", DisplayName = "Attribute Logical Name", Category = ColumnCategory.Core, DisplayOrder = 2, Width = 150, Description = "Logical name of the attribute" },
            new ColumnDefinition { Name = "AttributeDisplayName", DisplayName = "Attribute Display Name", Category = ColumnCategory.Core, DisplayOrder = 3, Width = 150, Description = "Display name of the attribute" },
            new ColumnDefinition { Name = "AttributeType", DisplayName = "Attribute Type", Category = ColumnCategory.Core, DisplayOrder = 4, Width = 100, Description = "Type of the attribute (String, Integer, Lookup, etc.)" },
            new ColumnDefinition { Name = "Required", DisplayName = "Required", Category = ColumnCategory.Core, DisplayOrder = 5, Width = 80, Description = "Whether the attribute is required" },
            new ColumnDefinition { Name = "MaxLength", DisplayName = "Max Length", Category = ColumnCategory.TypeSpecific, DisplayOrder = 6, Width = 90, Description = "Maximum length (for string/memo attributes)" },
            new ColumnDefinition { Name = "TypeSpecificDetails", DisplayName = "Type Details", Category = ColumnCategory.TypeSpecific, DisplayOrder = 7, Width = 300, Description = "Type-specific metadata (format, targets, options, etc.)" },
            new ColumnDefinition { Name = "Description", DisplayName = "Description", Category = ColumnCategory.TypeSpecific, DisplayOrder = 8, Width = 200, Description = "Attribute description" },

            // Identity (5)
            new ColumnDefinition { Name = "SchemaName", DisplayName = "Schema Name", Category = ColumnCategory.Identity, DisplayOrder = 9, Width = 150, Description = "Schema name of the attribute" },
            new ColumnDefinition { Name = "MetadataId", DisplayName = "Metadata ID", Category = ColumnCategory.Identity, DisplayOrder = 10, Width = 250, Description = "Unique metadata identifier (GUID)" },
            new ColumnDefinition { Name = "IsPrimaryId", DisplayName = "Is Primary ID", Category = ColumnCategory.Identity, DisplayOrder = 11, Width = 100, Description = "Whether this is the primary key field" },
            new ColumnDefinition { Name = "IsPrimaryName", DisplayName = "Is Primary Name", Category = ColumnCategory.Identity, DisplayOrder = 12, Width = 110, Description = "Whether this is the primary name field" },
            new ColumnDefinition { Name = "AttributeOf", DisplayName = "Attribute Of", Category = ColumnCategory.Identity, DisplayOrder = 13, Width = 150, Description = "Parent attribute (for calculated/virtual fields)" },

            // Metadata (4)
            new ColumnDefinition { Name = "IsManaged", DisplayName = "Is Managed", Category = ColumnCategory.Metadata, DisplayOrder = 14, Width = 90, Description = "Whether from a managed solution" },
            new ColumnDefinition { Name = "IsCustomAttribute", DisplayName = "Is Custom", Category = ColumnCategory.Metadata, DisplayOrder = 15, Width = 80, Description = "Whether this is a custom attribute" },
            new ColumnDefinition { Name = "IntroducedVersion", DisplayName = "Introduced Version", Category = ColumnCategory.Metadata, DisplayOrder = 16, Width = 130, Description = "Solution version where attribute was introduced" },
            new ColumnDefinition { Name = "AttributeTypeName", DisplayName = "Type Name", Category = ColumnCategory.Metadata, DisplayOrder = 17, Width = 120, Description = "Full type name" },

            // Behavior (4)
            new ColumnDefinition { Name = "IsValidForCreate", DisplayName = "Valid For Create", Category = ColumnCategory.Behavior, DisplayOrder = 18, Width = 110, Description = "Can be set when creating records" },
            new ColumnDefinition { Name = "IsValidForRead", DisplayName = "Valid For Read", Category = ColumnCategory.Behavior, DisplayOrder = 19, Width = 100, Description = "Can be retrieved" },
            new ColumnDefinition { Name = "IsValidForUpdate", DisplayName = "Valid For Update", Category = ColumnCategory.Behavior, DisplayOrder = 20, Width = 110, Description = "Can be updated" },
            new ColumnDefinition { Name = "IsLogical", DisplayName = "Is Logical", Category = ColumnCategory.Behavior, DisplayOrder = 21, Width = 80, Description = "Whether this is a virtual/calculated field" },

            // Security (5)
            new ColumnDefinition { Name = "IsSecured", DisplayName = "Is Secured", Category = ColumnCategory.Security, DisplayOrder = 22, Width = 90, Description = "Field-level security is enabled" },
            new ColumnDefinition { Name = "CanBeSecuredForCreate", DisplayName = "Can Secure Create", Category = ColumnCategory.Security, DisplayOrder = 23, Width = 120, Description = "Can prevent adding data with FLS" },
            new ColumnDefinition { Name = "CanBeSecuredForRead", DisplayName = "Can Secure Read", Category = ColumnCategory.Security, DisplayOrder = 24, Width = 120, Description = "Can prevent viewing data with FLS" },
            new ColumnDefinition { Name = "CanBeSecuredForUpdate", DisplayName = "Can Secure Update", Category = ColumnCategory.Security, DisplayOrder = 25, Width = 130, Description = "Can prevent updating data with FLS" },

            // Audit (1)
            new ColumnDefinition { Name = "IsAuditEnabled", DisplayName = "Audit Enabled", Category = ColumnCategory.Audit, DisplayOrder = 26, Width = 100, Description = "Whether auditing is enabled" },

            // Advanced (1)
            new ColumnDefinition { Name = "RequiredLevel", DisplayName = "Required Level", Category = ColumnCategory.Advanced, DisplayOrder = 27, Width = 120, Description = "Detailed requirement level (None, Recommended, ApplicationRequired, SystemRequired)" }
        };

        /// <summary>
        /// Get column configuration for a preset
        /// </summary>
        public static List<ColumnDefinition> GetPresetColumns(ColumnPreset preset)
        {
            var columns = AllColumns.Select(c => new ColumnDefinition
            {
                Name = c.Name,
                DisplayName = c.DisplayName,
                Category = c.Category,
                DisplayOrder = c.DisplayOrder,
                Width = c.Width,
                Description = c.Description,
                IsVisible = false
            }).ToList();

            switch (preset)
            {
                case ColumnPreset.Basic:
                    // Core 8 columns
                    SetVisible(columns, "TableLogicalName", "TableDisplayName", "AttributeLogicalName",
                        "AttributeDisplayName", "AttributeType", "Required", "TypeSpecificDetails", "Description");
                    break;

                case ColumnPreset.Standard:
                    // Basic + 8 more
                    SetVisible(columns, "TableLogicalName", "TableDisplayName", "AttributeLogicalName",
                        "AttributeDisplayName", "AttributeType", "Required", "MaxLength", "TypeSpecificDetails", "Description",
                        "SchemaName", "IsCustomAttribute", "IsPrimaryId", "IsValidForCreate", "IsValidForRead",
                        "IsValidForUpdate", "IntroducedVersion");
                    break;

                case ColumnPreset.Advanced:
                    // Standard + Security, Audit, Managed
                    SetVisible(columns, "TableLogicalName", "TableDisplayName", "AttributeLogicalName",
                        "AttributeDisplayName", "AttributeType", "Required", "MaxLength", "TypeSpecificDetails", "Description",
                        "SchemaName", "IsCustomAttribute", "IsPrimaryId", "IsPrimaryName", "IsValidForCreate",
                        "IsValidForRead", "IsValidForUpdate", "IntroducedVersion", "IsManaged", "IsLogical",
                        "AttributeOf", "IsSecured", "CanBeSecuredForCreate", "CanBeSecuredForRead",
                        "CanBeSecuredForUpdate", "IsAuditEnabled", "RequiredLevel");
                    break;

                case ColumnPreset.Full:
                    // All columns
                    foreach (var col in columns)
                        col.IsVisible = true;
                    break;
            }

            return columns;
        }

        private static void SetVisible(List<ColumnDefinition> columns, params string[] names)
        {
            foreach (var name in names)
            {
                var col = columns.FirstOrDefault(c => c.Name == name);
                if (col != null)
                    col.IsVisible = true;
            }
        }

        /// <summary>
        /// Load saved configuration from disk
        /// </summary>
        public static ColumnConfiguration LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var json = File.ReadAllText(ConfigFilePath);
                    return JsonConvert.DeserializeObject<ColumnConfiguration>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading column configuration: {ex.Message}");
            }

            // Return default (Standard preset)
            return new ColumnConfiguration
            {
                SelectedPreset = ColumnPreset.Standard,
                Columns = GetPresetColumns(ColumnPreset.Standard)
            };
        }

        /// <summary>
        /// Save configuration to disk
        /// </summary>
        public static void SaveConfiguration(ColumnConfiguration config)
        {
            try
            {
                if (!Directory.Exists(ConfigDirectory))
                    Directory.CreateDirectory(ConfigDirectory);

                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving column configuration: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Complete column configuration including preset, columns, sort state
    /// </summary>
    public class ColumnConfiguration
    {
        public ColumnPreset SelectedPreset { get; set; }
        public List<ColumnDefinition> Columns { get; set; }
        public string LastSortColumn { get; set; }
        public bool LastSortAscending { get; set; }

        /// <summary>
        /// Active filter criteria
        /// </summary>
        public FilterCriteria ActiveFilters { get; set; }

        public ColumnConfiguration()
        {
            ActiveFilters = new FilterCriteria();
        }
    }
}
