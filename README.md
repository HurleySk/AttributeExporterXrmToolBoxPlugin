# Attribute Metadata Exporter

XRM ToolBox plugin for exporting Dataverse/Dynamics 365 attribute metadata to CSV with advanced column management.

## Features

- **Data Sources**: Export from all entities or specific solutions
- **40+ Metadata Columns**: Organized into 7 categories (Core, Identity, Metadata, Behavior, Security, Audit, Advanced)
- **Column Presets**: Basic (8), Standard (16), Advanced (28), Full (40+), or Custom
- **Interactive Grid**: Real-time filtering, sortable columns, drag-drop reordering
- **Smart Type Details**: Automatically formats type-specific metadata (lookup targets, picklist options, string formats, etc.)
- **CSV Export**: Export filtered results with selected columns in custom order
- **Persistent Settings**: Column preferences, sort state, and filters saved across sessions

## Installation

### From XRM ToolBox (Recommended)
1. Open XRM ToolBox → **Tool Library** (Ctrl+T)
2. Search for "Attribute Metadata Exporter"
3. Click **Install** and restart

### From NuGet Package
1. Download `.nupkg` from [Releases](https://github.com/HurleySk/AttributeExporterXrmToolBoxPlugin/releases)
2. XRM ToolBox → **Plugins Store** → **Install from disk**
3. Select `.nupkg` file and restart

## Usage

1. **Connect** to Dataverse environment
2. **Choose scope**: All Entities or Selected Solution
3. **Click** "Load Attributes"
4. **Filter** using table/attribute name, type, required, custom, or primary ID filters
5. **Configure columns** (optional): Click "Columns..." to select preset or customize
6. **Export**: Click "Export to CSV"

### Column Presets

| Preset | Columns | Description |
|--------|---------|-------------|
| Basic | 8 | Table/attribute names, type, required, type details, description |
| Standard | 16 | Basic + schema name, custom flag, primary ID, validity flags, version |
| Advanced | 28 | Standard + security, audit, managed status, logical attributes |
| Full | 40+ | All available metadata properties |
| Custom | Variable | Select specific columns |

### Available Columns by Category

- **Core** (8): Table/attribute names, type, required, max length, type details, description
- **Identity** (5): Schema name, metadata ID, primary ID/name flags, attribute of
- **Metadata** (4): Managed, custom, introduced version, type name
- **Behavior** (4): Valid for create/read/update, is logical
- **Security** (4): Secured flags and can-be-secured permissions
- **Audit** (1): Audit enabled
- **Advanced** (1): Required level detail

## Configuration Storage

Settings saved to `%APPDATA%\AttributeExporter\config.json`:
- Column visibility, order, widths
- Active filters and sort state
- Selected preset

## Building from Source

```bash
git clone https://github.com/HurleySk/AttributeExporterXrmToolBoxPlugin.git
cd AttributeExporterXrmToolBoxPlugin
dotnet build --configuration Release
.\deploy.ps1 -Force
```

**Deploy script options**: `-SkipBuild`, `-Force`, `-WhatIf`

## Troubleshooting

**Plugin not appearing**: Delete `%APPDATA%\MscrmTools\XrmToolBox\Plugins\.manifestcache.dat` and restart XRM ToolBox

**"Object reference" errors**: Missing dependencies - re-run `deploy.ps1` to copy all DLLs

**Slow "All Entities" load**: Large environments take 30-60s - use "Selected Solution" or filters to narrow scope

**Export fails**: Ensure attributes loaded, at least one column visible, and CSV file not open in Excel

## Technical Details

**Dependencies**:
- XrmToolBoxPackage 1.2025.7.71
- Microsoft.CrmSdk.CoreAssemblies 9.0.2.59
- CsvHelper 12.3.2 (downgraded for .NET Framework 4.8)

**Requirements**: XRM ToolBox 2024.7.x+, .NET Framework 4.8

## Contributing

1. Fork repository
2. Create feature branch
3. Test in XRM ToolBox
4. Submit pull request

Follow existing code patterns, add XML docs, update README for user-facing changes.

## License

MIT License

## Author

**Samuel Hurley** - [HurleySk](https://github.com/HurleySk)

Built on [XRM ToolBox](https://www.xrmtoolbox.com/) by Tanguy Touzard

---

**Version 1.0.4** - UX improvements: Simplified filtering with always-visible filter panel
