# Attribute Metadata Exporter

**A comprehensive XRM ToolBox plugin for exporting Dataverse/Dynamics 365 attribute metadata to CSV with advanced column management.**

Export all metadata properties for attributes across your entire environment or from specific solutions. Configure which columns to display, reorder them with drag-drop, and save your preferences.

## Features

### Data Source Options
- **All Entities**: Export attributes from every table in your environment
- **Selected Solution**: Export attributes only from tables in a specific unmanaged solution
- **Load on Demand**: Efficient metadata retrieval with async loading

### Advanced Column Management
- **40+ Metadata Columns**: Export comprehensive attribute metadata organized into 7 categories
- **4 Column Presets**: Quick configurations (Basic, Standard, Advanced, Full)
- **Custom Columns**: Pick exactly which columns you want to see and export
- **Column Configuration UI**: User-friendly dialog for managing column visibility
- **Persistent Settings**: Your column preferences are saved across sessions

### Interactive Data Grid
- **Search & Filter**: Real-time filtering across all visible columns
- **Sortable Columns**: Click any header to sort ascending/descending
- **Drag-Drop Reordering**: Rearrange columns by dragging headers
- **Horizontal Scrolling**: View many columns without cramping
- **Smart Details Column**: Type-specific metadata automatically formatted per attribute type

### Export Capabilities
- **CSV Export**: Clean, formatted CSV with only your selected columns
- **Custom Column Order**: Export respects your current column arrangement
- **Filtered Export**: Export only what's visible after filtering
- **Timestamped Files**: Auto-generated filenames (e.g., `AttributeMetadata_20250930_161500.csv`)

## Installation

### Prerequisites
- **XRM ToolBox** (version 2024.7.x or later)
- **.NET Framework 4.8** (installed with Windows 10/11)
- **Dataverse/Dynamics 365 environment** with sufficient read permissions

### Option 1: Using Deploy Script (Recommended for Development)
```powershell
# Clone repository
git clone https://github.com/HurleySk/AttributeExporterXrmToolBoxPlugin.git
cd AttributeExporterXrmToolBoxPlugin

# Build and deploy
.\deploy.ps1 -Force
```

The deploy script will:
- Close XRM ToolBox if running
- Build the plugin
- Copy files to `%APPDATA%\MscrmTools\XrmToolBox\Plugins\`
- Clean old plugin files from OneDrive locations
- Verify deployment

### Option 2: Manual Installation
1. Download the latest release or build from source
2. Copy these files to `%APPDATA%\MscrmTools\XrmToolBox\Plugins\`:
   - `AttributeExporterXrmToolBoxPlugin.dll`
   - `CsvHelper.dll`
   - `Microsoft.Bcl.AsyncInterfaces.dll`
   - `System.Threading.Tasks.Extensions.dll`
3. Restart XRM ToolBox
4. Look for "Attribute Metadata Exporter" in the Tools menu

## Usage

### Basic Workflow

1. **Connect to Environment**
   - Open XRM ToolBox
   - Connect to your Dataverse/Dynamics 365 environment
   - Launch "Attribute Metadata Exporter"

2. **Choose Data Source**
   - Select **All Entities** to export from entire environment
   - OR select **Selected Solution** and choose a solution from dropdown
   - Click **Load Attributes**

3. **Configure Columns** (Optional)
   - Click **Columns...** button
   - Choose a preset or customize individual columns
   - Drag column headers to reorder
   - Click OK to apply

4. **Filter & Search** (Optional)
   - Type in search box to filter attributes
   - Filter applies across all visible columns

5. **Export to CSV**
   - Click **Export to CSV**
   - Choose save location
   - CSV includes only visible columns in current order

### Column Presets

| Preset | Columns | Description |
|--------|---------|-------------|
| **Basic** | 8 | Core identification: table/attribute names, type, required, type details, description |
| **Standard** | 16 | Basic + schema name, custom flag, primary ID, validity flags, version |
| **Advanced** | 28 | Standard + security, audit, managed status, logical attributes |
| **Full** | 40+ | Every available metadata property |
| **Custom** | Variable | Select exactly which columns you want |

### Available Metadata Columns

#### Core (8 columns)
- Table Logical Name, Table Display Name
- Attribute Logical Name, Attribute Display Name
- Attribute Type, Required
- Type Details (smart column), Description

#### Identity (5 columns)
- Schema Name, Metadata ID
- Is Primary ID, Is Primary Name, Attribute Of

#### Metadata (4 columns)
- Is Managed, Is Custom, Introduced Version, Type Name

#### Behavior (4 columns)
- Valid For Create, Valid For Read, Valid For Update, Is Logical

#### Security (4 columns)
- Is Secured, Can Secure Create, Can Secure Read, Can Secure Update

#### Audit (1 column)
- Audit Enabled

#### Type-Specific (3 columns)
- Max Length, Type Details, Description

#### Advanced (1 column)
- Required Level (detailed)

### Smart Type Details Column

The **Type Details** column automatically shows relevant metadata based on attribute type:

- **String**: `Format: Email, MaxLen: 100, Localizable`
- **Lookup**: `Targets: account, contact, lead`
- **DateTime**: `Behavior: UserLocal, Format: DateAndTime`
- **Picklist**: `Options: 5 choices, OptionSet: new_status`
- **Boolean**: `Labels: Yes / No, Default: No`
- **Integer**: `Range: 0 to 100, Format: None`
- **Decimal/Money**: `Precision: 2, Range: 0.00 to 1000000.00`

This eliminates the need for 10+ sparse columns that only apply to specific attribute types.

## Technical Details

### Architecture

```
AttributeExporterXrmToolBoxPlugin/
├── AttributeExporterPlugin.cs          # Plugin entry point (MEF export)
├── AttributeExporterControl.cs         # Main UI control
├── Models/
│   ├── AttributeMetadataInfo.cs        # 28 metadata properties
│   ├── ColumnDefinition.cs             # Column configuration model
│   ├── ColumnPreset.cs                 # Preset enum
│   └── SolutionInfo.cs                 # Solution data model
├── Services/
│   ├── ColumnConfigurationService.cs   # Preset management, persistence
│   └── MetadataFormatter.cs            # Smart TypeSpecificDetails logic
├── Forms/
│   └── ColumnConfigurationDialog.cs    # Column picker UI
├── Resources/
│   └── logo64.png                      # Plugin icon
└── deploy.ps1                          # Automated deployment script
```

### Dependencies

- **XrmToolBoxPackage** 1.2025.7.71 - Plugin infrastructure
- **MscrmTools.Xrm.Connection** 1.2025.7.63 - Connection management
- **Microsoft.CrmSdk.CoreAssemblies** 9.0.2.59 - Dynamics 365 SDK
- **CsvHelper** 12.3.2 - CSV export (downgraded for .NET Framework 4.8 compatibility)
- **System.Windows.Forms** - UI framework

### Configuration Storage

User preferences are saved to:
```
%APPDATA%\AttributeExporter\config.json
```

Contains:
- Selected preset (Basic/Standard/Advanced/Full/Custom)
- Column visibility for each of 28 columns
- Column display order
- Column widths
- Last sort column and direction

### Supported Attribute Types

All Dataverse attribute types are supported:
- String, Memo
- Integer, Decimal, Double, Money
- Boolean (Two Options)
- DateTime
- Lookup, Customer, Owner
- Picklist, State, Status, MultiSelectPicklist
- Image, File
- Uniqueidentifier, Virtual, EntityName, ManagedProperty

## Building from Source

### Prerequisites
- Visual Studio 2022 or later
- .NET Framework 4.8 SDK
- PowerShell 5.1+ (for deploy script)

### Build Steps

```bash
# Clone repository
git clone https://github.com/HurleySk/AttributeExporterXrmToolBoxPlugin.git
cd AttributeExporterXrmToolBoxPlugin

# Restore NuGet packages
dotnet restore

# Build Release configuration
dotnet build --configuration Release

# Deploy (optional)
.\deploy.ps1 -Force
```

### Deploy Script Parameters

```powershell
# Skip build, just deploy existing binaries
.\deploy.ps1 -SkipBuild

# Auto-close XRM ToolBox if running
.\deploy.ps1 -Force

# Show what would be deployed without doing it
.\deploy.ps1 -WhatIf
```

## Troubleshooting

### Plugin Doesn't Appear in XRM ToolBox

1. Check deployment path: `%APPDATA%\MscrmTools\XrmToolBox\Plugins\`
2. Verify all DLL dependencies are present
3. Delete manifest cache: `%APPDATA%\MscrmTools\XrmToolBox\Plugins\.manifestcache.dat`
4. Check for old plugin versions in OneDrive locations
5. Restart XRM ToolBox

### "Object reference not set to an instance of an object"

- Usually indicates missing DLL dependencies
- Re-run `deploy.ps1` to ensure all files are copied
- Check that `CsvHelper.dll`, `Microsoft.Bcl.AsyncInterfaces.dll`, and `System.Threading.Tasks.Extensions.dll` are present

### Column Configuration Dialog Shows Type Names Instead of Display Names

- Fixed in latest version
- Update to latest build
- Clear XRM ToolBox cache

### Solution Dropdown Always Has a Selection

- Fixed in latest version
- Use "All Entities" radio button first, then "Selected Solution"
- Selection is properly cleared when switching to "All Entities"

### Export Fails or CSV is Empty

- Ensure attributes are loaded (click "Load Attributes" first)
- Check that at least one column is visible
- Verify write permissions to export location
- Don't have CSV file open in Excel during export

### Slow Performance with "All Entities"

- Large environments may take 30-60 seconds to load all metadata
- Use search/filter to reduce visible rows
- Consider using "Selected Solution" for specific tables
- Progress dialog shows loading status

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Test thoroughly in XRM ToolBox
5. Commit with descriptive messages
6. Push to your fork
7. Open a Pull Request

### Development Guidelines

- Follow existing code patterns and naming conventions
- Add XML documentation comments to public APIs
- Test with multiple Dataverse environments
- Update README.md if adding user-facing features
- Use deploy.ps1 for testing deployments

## License

MIT License - see LICENSE file for details.

## Author

**Samuel Hurley** - [HurleySk](https://github.com/HurleySk)
Procentrix, Inc.

## Acknowledgments

- Built on [XRM ToolBox](https://www.xrmtoolbox.com/) by Tanguy Touzard
- Uses [CsvHelper](https://joshclose.github.io/CsvHelper/) by Josh Close
- Powered by Microsoft Dataverse SDK

## Version History

### v1.0.0 (Current)
- ✅ All Entities and Solution-based export
- ✅ 40+ metadata columns with 7 categories
- ✅ 4 column presets + custom configuration
- ✅ Drag-drop column reordering
- ✅ Sortable columns
- ✅ Search & filter
- ✅ Smart TypeSpecificDetails column
- ✅ Persistent column configuration
- ✅ CSV export with configurable columns