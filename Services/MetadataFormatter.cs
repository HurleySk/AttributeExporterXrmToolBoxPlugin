using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributeExporterXrmToolBoxPlugin.Services
{
    /// <summary>
    /// Formats attribute metadata into human-readable strings
    /// </summary>
    public static class MetadataFormatter
    {
        /// <summary>
        /// Get intelligently formatted type-specific details based on attribute type
        /// </summary>
        public static string GetTypeSpecificDetails(AttributeMetadata attribute)
        {
            if (attribute == null)
                return string.Empty;

            var parts = new List<string>();

            switch (attribute)
            {
                case StringAttributeMetadata stringAttr:
                    if (stringAttr.Format.HasValue)
                        parts.Add($"Format: {stringAttr.Format}");
                    if (stringAttr.FormatName != null)
                        parts.Add($"FormatName: {stringAttr.FormatName.Value}");
                    if (stringAttr.MaxLength.HasValue)
                        parts.Add($"MaxLen: {stringAttr.MaxLength}");
                    if (stringAttr.IsLocalizable.HasValue && stringAttr.IsLocalizable.Value)
                        parts.Add("Localizable");
                    if (!string.IsNullOrWhiteSpace(stringAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(stringAttr.FormulaDefinition)}");
                    break;

                case MemoAttributeMetadata memoAttr:
                    if (memoAttr.Format.HasValue)
                        parts.Add($"Format: {memoAttr.Format}");
                    if (memoAttr.MaxLength.HasValue)
                        parts.Add($"MaxLen: {memoAttr.MaxLength}");
                    if (memoAttr.IsLocalizable.HasValue && memoAttr.IsLocalizable.Value)
                        parts.Add("Localizable");
                    break;

                case LookupAttributeMetadata lookupAttr:
                    if (lookupAttr.Targets != null && lookupAttr.Targets.Length > 0)
                        parts.Add($"Targets: {string.Join(", ", lookupAttr.Targets)}");
                    break;

                case DateTimeAttributeMetadata dateTimeAttr:
                    if (dateTimeAttr.DateTimeBehavior != null)
                        parts.Add($"Behavior: {dateTimeAttr.DateTimeBehavior.Value}");
                    if (dateTimeAttr.Format.HasValue)
                        parts.Add($"Format: {dateTimeAttr.Format}");
                    if (!string.IsNullOrWhiteSpace(dateTimeAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(dateTimeAttr.FormulaDefinition)}");
                    break;

                case IntegerAttributeMetadata intAttr:
                    if (intAttr.MinValue.HasValue || intAttr.MaxValue.HasValue)
                        parts.Add($"Range: {intAttr.MinValue?.ToString() ?? "∞"} to {intAttr.MaxValue?.ToString() ?? "∞"}");
                    if (intAttr.Format.HasValue)
                        parts.Add($"Format: {intAttr.Format}");
                    if (!string.IsNullOrWhiteSpace(intAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(intAttr.FormulaDefinition)}");
                    break;

                case DecimalAttributeMetadata decimalAttr:
                    if (decimalAttr.Precision.HasValue)
                        parts.Add($"Precision: {decimalAttr.Precision}");
                    if (decimalAttr.MinValue.HasValue || decimalAttr.MaxValue.HasValue)
                        parts.Add($"Range: {decimalAttr.MinValue?.ToString() ?? "∞"} to {decimalAttr.MaxValue?.ToString() ?? "∞"}");
                    if (!string.IsNullOrWhiteSpace(decimalAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(decimalAttr.FormulaDefinition)}");
                    break;

                case MoneyAttributeMetadata moneyAttr:
                    if (moneyAttr.Precision.HasValue)
                        parts.Add($"Precision: {moneyAttr.Precision}");
                    if (moneyAttr.MinValue.HasValue || moneyAttr.MaxValue.HasValue)
                        parts.Add($"Range: {moneyAttr.MinValue?.ToString("N2") ?? "∞"} to {moneyAttr.MaxValue?.ToString("N2") ?? "∞"}");
                    if (moneyAttr.PrecisionSource.HasValue)
                        parts.Add($"PrecisionSource: {moneyAttr.PrecisionSource}");
                    if (!string.IsNullOrWhiteSpace(moneyAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(moneyAttr.FormulaDefinition)}");
                    break;

                case DoubleAttributeMetadata doubleAttr:
                    if (doubleAttr.Precision.HasValue)
                        parts.Add($"Precision: {doubleAttr.Precision}");
                    if (doubleAttr.MinValue.HasValue || doubleAttr.MaxValue.HasValue)
                        parts.Add($"Range: {doubleAttr.MinValue?.ToString() ?? "∞"} to {doubleAttr.MaxValue?.ToString() ?? "∞"}");
                    if (!string.IsNullOrWhiteSpace(doubleAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(doubleAttr.FormulaDefinition)}");
                    break;

                case PicklistAttributeMetadata picklistAttr:
                    if (picklistAttr.OptionSet != null)
                    {
                        var optionCount = picklistAttr.OptionSet.Options?.Count ?? 0;
                        parts.Add($"Options: {optionCount} choices");
                        if (!string.IsNullOrWhiteSpace(picklistAttr.OptionSet.Name))
                            parts.Add($"OptionSet: {picklistAttr.OptionSet.Name}");
                        if (picklistAttr.DefaultFormValue.HasValue)
                            parts.Add($"Default: {picklistAttr.DefaultFormValue}");
                    }
                    if (!string.IsNullOrWhiteSpace(picklistAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(picklistAttr.FormulaDefinition)}");
                    break;

                case MultiSelectPicklistAttributeMetadata multiSelectAttr:
                    if (multiSelectAttr.OptionSet != null)
                    {
                        var optionCount = multiSelectAttr.OptionSet.Options?.Count ?? 0;
                        parts.Add($"Options: {optionCount} choices (Multi-Select)");
                        if (!string.IsNullOrWhiteSpace(multiSelectAttr.OptionSet.Name))
                            parts.Add($"OptionSet: {multiSelectAttr.OptionSet.Name}");
                    }
                    if (!string.IsNullOrWhiteSpace(multiSelectAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(multiSelectAttr.FormulaDefinition)}");
                    break;

                case BooleanAttributeMetadata boolAttr:
                    if (boolAttr.OptionSet != null)
                    {
                        var trueLabel = boolAttr.OptionSet.TrueOption?.Label?.UserLocalizedLabel?.Label ?? "True";
                        var falseLabel = boolAttr.OptionSet.FalseOption?.Label?.UserLocalizedLabel?.Label ?? "False";
                        parts.Add($"Labels: {trueLabel} / {falseLabel}");
                        if (boolAttr.DefaultValue.HasValue)
                            parts.Add($"Default: {(boolAttr.DefaultValue.Value ? trueLabel : falseLabel)}");
                    }
                    if (!string.IsNullOrWhiteSpace(boolAttr.FormulaDefinition))
                        parts.Add($"Formula: {TruncateFormula(boolAttr.FormulaDefinition)}");
                    break;

                case StateAttributeMetadata stateAttr:
                    if (stateAttr.OptionSet != null)
                    {
                        var optionCount = stateAttr.OptionSet.Options?.Count ?? 0;
                        parts.Add($"States: {optionCount}");
                    }
                    break;

                case StatusAttributeMetadata statusAttr:
                    if (statusAttr.OptionSet != null)
                    {
                        var optionCount = statusAttr.OptionSet.Options?.Count ?? 0;
                        parts.Add($"Status Codes: {optionCount}");
                    }
                    break;

                case ImageAttributeMetadata imageAttr:
                    parts.Add("Image Attribute");
                    if (imageAttr.MaxHeight.HasValue)
                        parts.Add($"MaxHeight: {imageAttr.MaxHeight}");
                    if (imageAttr.MaxWidth.HasValue)
                        parts.Add($"MaxWidth: {imageAttr.MaxWidth}");
                    break;

                case FileAttributeMetadata fileAttr:
                    parts.Add("File Attribute");
                    if (fileAttr.MaxSizeInKB.HasValue)
                        parts.Add($"MaxSize: {fileAttr.MaxSizeInKB} KB");
                    break;
            }

            return parts.Count > 0 ? string.Join(", ", parts) : string.Empty;
        }

        private static string TruncateFormula(string formula, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(formula))
                return string.Empty;

            formula = formula.Replace("\r\n", " ").Replace("\n", " ").Trim();
            return formula.Length > maxLength ? formula.Substring(0, maxLength) + "..." : formula;
        }
    }
}
