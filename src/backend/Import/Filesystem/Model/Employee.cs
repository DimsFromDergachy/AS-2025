using Ganss.Excel;
using System.Text.Json.Serialization;

namespace AS_2025.Import.Filesystem.Model;

public record Employee
{
    [Column("Id")]
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [Column("FirstName")]
    [JsonPropertyName("firstName")]
    public string FirstName { get; init; } = string.Empty;

    [Column("LastName")]
    [JsonPropertyName("lastName")]
    public string LastName { get; init; } = string.Empty;

    [Column("Type")]
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [Column("Level")]
    [JsonPropertyName("level")]
    public string Level { get; init; } = string.Empty;

    [Column("HireDate")]
    [JsonPropertyName("hireDate")]
    public string HireDate { get; init; } = string.Empty;

    [Column("Salary")]
    [JsonPropertyName("salary")]
    public string Salary { get; init; } = string.Empty;

    [Column("ManagerId")]
    [JsonPropertyName("managerId")]
    public string ManagerId { get; init; } = string.Empty;

    [Column("TeamId")]
    [JsonPropertyName("teamId")]
    public string TeamId { get; init; } = string.Empty;

    [Column("DepartmentId")]
    [JsonPropertyName("departmentId")]
    public string DepartmentId { get; init; } = string.Empty;
}