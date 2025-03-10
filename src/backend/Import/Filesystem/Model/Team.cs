using Ganss.Excel;

namespace AS_2025.Import.Filesystem.Model;

public record Team
{
    [Column("Id")]
    public string Id { get; init; } = string.Empty;

    [Column("Name")]
    public string Name { get; init; } = string.Empty;

    [Column("Type")]
    public string Type { get; init; } = string.Empty;

    [Column("DepartmentId")]
    public string DepartmentId { get; init; } = string.Empty;

    [Column("TeamLeadId")]
    public string TeamLeadId { get; init; } = string.Empty;
}