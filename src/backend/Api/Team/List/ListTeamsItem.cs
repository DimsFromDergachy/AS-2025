namespace AS_2025.Api.Team.List;

public record ListTeamsItem
{
    public Guid Id { get; init; }

    public string Url { get; init; }

    public string Name { get; init; }

    public string Type { get; init; }

    public string DepartmentName { get; init; }

    public string TeamLead { get; init; }

    public int MembersCount { get; init; }

    public int AssignedProjectsCount { get; init; }
}