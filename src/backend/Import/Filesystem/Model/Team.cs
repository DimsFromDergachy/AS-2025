using Ganss.Excel;
using System.Text.Json.Serialization;

namespace AS_2025.Import.Filesystem.Model;

public record Team
{
    [Column("Id")]
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [Column("Name")]
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [Column("Type")]
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [Column("DepartmentId")]
    [JsonPropertyName("departmentId")]
    public string DepartmentId { get; init; } = string.Empty;

    [Column("TeamLeadId")]
    [JsonPropertyName("teamLeadId")]
    public string TeamLeadId { get; init; } = string.Empty;
}