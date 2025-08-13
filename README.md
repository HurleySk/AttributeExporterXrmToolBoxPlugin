# Attribute Metadata Exporter - XRM ToolBox Plugin

A powerful XRM ToolBox plugin for exporting attribute metadata from Dynamics 365/Power Platform solutions to CSV format.

## Features

- **Solution Selection**: Choose from available unmanaged solutions in your environment
- **Comprehensive Attribute Export**: Export all attributes from entities within the selected solution
- **Rich Metadata**: Includes table logical names, display names, attribute types, requirements, and more
- **Preview Grid**: Review all attributes before export in a detailed data grid
- **CSV Export**: Clean, formatted CSV output with all attribute metadata
- **User-Friendly Interface**: Intuitive Windows Forms interface with clear controls

## What Gets Exported

The plugin exports the following attribute metadata for each attribute:

- **Table Logical Name**: The internal logical name of the entity/table
- **Table Display Name**: The user-friendly display name of the entity/table
- **Attribute Logical Name**: The internal logical name of the attribute
- **Attribute Display Name**: The user-friendly display name of the attribute
- **Attribute Type**: The type of attribute (e.g., String, Integer, Lookup, etc.)
- **Required**: Whether the attribute is required
- **Max Length**: Maximum length for string/memo attributes
- **Description**: Any description text associated with the attribute

## Installation

### Prerequisites

- XRM ToolBox (latest version recommended)
- .NET 6.0 or later
- Access to a Dynamics 365/Power Platform environment

### Build and Install

1. **Clone or download** this repository
2. **Build the solution** using Visual Studio or `dotnet build`
3. **Copy the built DLL** to your XRM ToolBox plugins folder:
   - Usually located at: `%APPDATA%\MscrmTools\XrmToolBox\Plugins\`
4. **Restart XRM ToolBox**
5. **Connect to your environment** and look for "Attribute Metadata Exporter" in the plugins list

## Usage

### Step 1: Connect to Environment
1. Open XRM ToolBox
2. Connect to your Dynamics 365/Power Platform environment
3. Launch the "Attribute Metadata Exporter" plugin

### Step 2: Select Solution
1. Choose a solution from the dropdown (only unmanaged solutions are shown)
2. Click "Load Attributes" to retrieve all attributes from entities in that solution

### Step 3: Review Attributes
1. View all attributes in the data grid
2. Check the attribute count at the top of the grid
3. Review the metadata to ensure it's what you want to export

### Step 4: Export to CSV
1. Click "Export to CSV"
2. Choose a save location and filename
3. The file will be saved with a timestamp (e.g., `AttributeMetadata_20241201_143022.csv`)

## Technical Details

### Architecture
- **Plugin Class**: `AttributeExporterPlugin` - Main entry point implementing IXrmToolBoxPlugin
- **Form Class**: `AttributeExporterForm` - Main user interface
- **Data Models**: 
  - `AttributeMetadataInfo` - Attribute metadata structure
  - `SolutionInfo` - Solution information structure

### Dependencies
- **McTools.Xrm.Connection**: For XRM ToolBox integration
- **Microsoft.Xrm.Sdk**: For Dynamics 365 SDK functionality
- **CsvHelper**: For CSV export functionality
- **System.Windows.Forms**: For the user interface

### Supported Attribute Types
The plugin handles all standard Dynamics 365 attribute types including:
- String attributes
- Integer attributes
- Decimal attributes
- DateTime attributes
- Lookup attributes
- Option set attributes
- Boolean attributes
- And more...

## Troubleshooting

### Common Issues

1. **"Connection Required" Error**
   - Ensure you're connected to an environment in XRM ToolBox
   - Check that your connection has sufficient permissions

2. **No Solutions Available**
   - Verify you have access to unmanaged solutions
   - Check that solutions are marked as visible

3. **Export Fails**
   - Ensure you have write permissions to the selected folder
   - Check that the CSV file isn't open in another application

4. **Slow Performance**
   - Large solutions with many entities may take time to load
   - Consider filtering to specific entities if needed

### Performance Tips

- The plugin loads all attributes at once for better user experience
- For very large solutions, consider breaking them down into smaller components
- CSV export is optimized for large datasets

## Development

### Building from Source

```bash
# Clone the repository
git clone https://github.com/yourusername/AttributeExporterXrmToolBoxPlugin.git

# Navigate to the project directory
cd AttributeExporterXrmToolBoxPlugin

# Restore packages
dotnet restore

# Build the project
dotnet build

# Run tests (if available)
dotnet test
```

### Project Structure

```
AttributeExporterXrmToolBoxPlugin/
├── AttributeExporterPlugin.cs          # Main plugin class
├── AttributeExporterForm.cs            # Main form logic
├── AttributeExporterForm.Designer.cs   # Form designer
├── Models/                             # Data models
│   ├── AttributeMetadataInfo.cs
│   └── SolutionInfo.cs
├── Properties/                         # Assembly properties
│   ├── AssemblyInfo.cs
│   ├── Resources.resx
│   └── Resources.Designer.cs
├── Resources/                          # Plugin resources
│   └── icon.png
├── AttributeExporterXrmToolBoxPlugin.csproj
└── README.md
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or feature requests:
- Create an issue on GitHub
- Contact the development team
- Check the XRM ToolBox community forums

## Version History

- **v1.0.0** - Initial release with basic attribute export functionality
  - Solution selection
  - Attribute metadata retrieval
  - CSV export
  - User-friendly interface

## Roadmap

- [ ] Filtering options (by entity, attribute type, etc.)
- [ ] Custom field selection
- [ ] Multiple format export (Excel, JSON)
- [ ] Batch processing for large solutions
- [ ] Attribute dependency analysis
- [ ] Custom metadata field support
