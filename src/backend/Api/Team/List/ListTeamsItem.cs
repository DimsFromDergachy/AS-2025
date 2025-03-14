using AS_2025.Domain.Common;
using AS_2025.Schema.List;

namespace AS_2025.Api.Team.List;

public record ListTeamsItem
{
    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Order = 1)]
    public string Name { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Tag, TagReferenceEnum = nameof(TeamType), Order = 2)]
    public string Type { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Order = 3)]
    public string DepartmentName { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Order = 4)]
    public string TeamLead { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Integer, Order = 5)]
    public int MembersCount { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Integer, Order = 6)]
    public int AssignedProjectsCount { get; init; }
}
