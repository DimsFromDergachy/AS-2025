using System.ComponentModel;
using Newtonsoft.Json.Converters;

namespace AS_2025.Export;

[TypeConverter(typeof(StringEnumConverter))]
public enum ExportType
{
    Excel,
    Html
}