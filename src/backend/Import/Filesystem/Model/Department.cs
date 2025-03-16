using System.Text.Json.Serialization;
using Ganss.Excel;

namespace AS_2025.Import.Filesystem.Model;

public record Department
{
    [Column("Id")]
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [Column("Name")]
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}