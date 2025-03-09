using Ganss.Excel;

namespace AS_2025.Import.Filesystem.Model;

public record Department
{
    [Column("Id")]
    public string Id { get; init; } = string.Empty;

    [Column("Name")]
    public string Name { get; init; } = string.Empty;
}