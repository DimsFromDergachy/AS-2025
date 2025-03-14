using Ganss.Excel;

namespace AS_2025.Import.Filesystem.Model;

public record Employee
{
    [Column("Id")]
    public string Id { get; init; } = string.Empty;

    [Column("FirstName")]
    public string FirstName { get; init; } = string.Empty;

    [Column("LastName")]
    public string LastName { get; init; } = string.Empty;

    [Column("Type")]
    public string Type { get; init; } = string.Empty;

    [Column("Level")]
    public string Level { get; init; } = string.Empty;

    [Column("HireDate")]
    public string HireDate { get; init; } = string.Empty;

    [Column("Salary")]
    public string Salary { get; init; } = string.Empty;

    [Column("ManagerId")]
    public string ManagerId { get; init; } = string.Empty;

    [Column("TeamId")]
    public string TeamId { get; init; } = string.Empty;

    [Column("DepartmentId")]
    public string DepartmentId { get; init; } = string.Empty;
}