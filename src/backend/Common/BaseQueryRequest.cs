using MediatR;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AS_2025.Common;

public abstract record BaseQueryRequest<T> : IParsable<T> where T : IBaseRequest, IParsable<T>?, new()
{
    public static T Parse(string query, IFormatProvider? provider)
    {
        return TryParse(query, provider, out var request) ? request : new T();
    }

    public static bool TryParse(string? query, IFormatProvider? provider, [NotNullWhen(true)] out T? result)
    {
        result = default;

        if (query is null)
        {
            return false;
        }

        var decoded = Uri.UnescapeDataString(query);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        result = JsonSerializer.Deserialize<T>(decoded, options);

        return result is not null;
    }
}