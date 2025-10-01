# Attribute Metadata Exporter

XRM ToolBox plugin for exporting Dataverse/Dynamics 365 attribute metadata to CSV with advanced column management and enhanced data interaction.

## Features

### Data Management
- **Data Sources**: Export from all entities or specific solution-based attributes
- **40+ Metadata Columns**: Organized into 7 categories (Core, Identity, Metadata, Behavior, Security, Audit, Advanced)
- **Column Presets**: Basic (8), Standard (16), Advanced (28), Full (40+), or Custom
- **Smart Type Details**: Automatically formats type-specific metadata (lookup targets, picklist options, string formats, etc.)
- **CSV Export**: Export filtered results with selected columns in custom order

### Interactive Grid
- **Flexible Selection**: Cell-level and full-row selection modes
- **Multi-Select**: Ctrl+Click for multiple selections, Shift+Click for range selection
- **Copy Support**: Right-click context menu or Ctrl+C to copy selected cells/rows
- **Cell Viewer**: Double-click any cell to view full content with copy button for long values
- **Dynamic Counter**: Real-time display of selected attribute count
- **Sortable Columns**: Click headers to sort, with persistent sort state
- **Drag-Drop Reordering**: Customize column order directly in grid

### Filtering & Search
- **Real-Time Filters**: Table name, attribute name, type, required level, custom/managed, primary ID
- **Always-Visible Panel**: Expanded filter controls for quick access
- **Session-Based**: Filters reset on new loads for clean slate each time
- **Total Counter**: Always displays total attribute count

### Productivity
- **Reload Solutions**: Refresh solution list without reconnecting
- **Smart Button Text**: "Load Attributes" changes to "Reload Attributes" after first load
- **Performance Optimized**: Batch loading with ExecuteMultipleRequest for faster solution queries
- **Persistent Settings**: Column preferences and widths saved across sessions

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

### Basic Workflow

1. **Connect** to Dataverse environment
2. **Choose scope**: All Entities or Selected Solution
3. **Click** "Load Attributes" (becomes "Reload Attributes" after first load)
4. **Filter** using table/attribute name, type, required, custom, or primary ID filters
5. **Configure columns** (optional): Click "Columns..." to select preset or customize
6. **Export**: Click "Export to CSV"

### Working with Data

#### Selection & Copying
- **Select cells**: Click individual cells or drag across multiple cells
- **Select rows**: Click row header to select full row
- **Multi-select**: Hold Ctrl and click for multiple selections
- **Range select**: Hold Shift and click row headers for range selection
- **Copy data**:
  - Right-click → "Copy Cell(s)" or "Copy Full Row(s)"
  - Press Ctrl+C to copy selected cells
  - Paste into Excel, Word, or any text editor
- **View long text**: Double-click any cell to open viewer with "Copy Text" button

#### Managing Solutions
- **Reload Solutions**: Click "Reload Solutions" button to refresh list without reconnecting
- **Filter by solution**: Only solutions containing attributes appear in dropdown
- **Performance**: Solution loading uses batch requests for optimal speed

#### Filtering Data
- **Apply filters**: Type in text boxes or select from dropdowns - results update in real-time
- **Clear filters**: Click "Clear Filters" button
- **Session-based**: Filters reset when loading new data or changing scope
- **Counters**:
  - "Total Attributes: X" shows loaded count
  - "Selected Attributes: Y" updates as you select rows/cells

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
- Sort state (column and direction)
- Selected preset

**Note**: Filters are session-based and do NOT persist - each load starts fresh.

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

**Slow "All Entities" load**: Large environments take 30-60s - use "Selected Solution" for faster loading

**Export fails**: Ensure attributes loaded, at least one column visible, and CSV file not open in Excel

**Solution shows 0 attributes**: Only solutions containing attribute components (componenttype=2) appear - verify solution has attribute customizations

**Copy/Paste not working**: Ensure cells are selected (highlighted) before pressing Ctrl+C or using context menu

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

## Release Notes

### Version 2.0.0 (Latest)
**Major Release - Enhanced Data Interaction**

- Cell-level selection with multi-select (Ctrl+Click, Shift+Click)
- Copy support via context menu and Ctrl+C
- Double-click cell viewer with copy button for long values
- Reload Solutions button for quick refresh
- Dynamic button text ("Load" → "Reload Attributes")
- Performance: Batch loading for solution attributes (30x faster)
- Real-time selected attribute counter
- Session-based filters (no persistence)
