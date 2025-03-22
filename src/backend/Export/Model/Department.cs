using Ganss.Excel;

namespace AS_2025.Export.Model;

public record Department
{
    [Column("Id")]
    public Guid Id { get; init; }

    [Column("Url")]
    public string Url { get; init; }

    [Column("Name")]
    public string Name { get; init; }

    [Column("Head")]
    public string Head { get; init; }

    [Column("TeamsCount")]
    public int TeamsCount { get; init; }

    [Column("EmployeesCount")]
    public int EmployeesCount { get; init; }
}