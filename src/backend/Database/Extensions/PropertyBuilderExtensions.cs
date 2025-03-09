using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AS_2025.Database.Extensions;

public static class PropertyBuilderExtensions
{
    private static readonly JsonSerializerOptions ConvertToProviderSerializerOptions = new()
    {
        WriteIndented = false
    };

    private static readonly JsonSerializerOptions ConvertFromProviderSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
    {
        return propertyBuilder.HasConversion(
            v => JsonSerializer.Serialize(v, ConvertToProviderSerializerOptions),
            v => string.IsNullOrEmpty(v) ? default : JsonSerializer.Deserialize<T>(v, ConvertFromProviderSerializerOptions)
        );
    }
}