﻿using AS_2025.Api.Team.List;
using AS_2025.Common;
using AS_2025.Domain.Common;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Team;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Team.Id), nameof(ListTeamsItem.Url), Use = nameof(GuidToUrl))]
    [MapProperty("Type", "Type", Use = nameof(TeamTypeToString))]
    [MapProperty("Department.Name", "DepartmentName")]
    [MapProperty("TeamLead.FullName", "TeamLead")]
    [MapProperty("Members", "MembersCount", Use = nameof(MembersToCount))]
    [MapProperty("AssignedProjects", "AssignedProjectsCount", Use = nameof(AssignedProjectsToCount))]
    public static partial ListTeamsItem ToListItem(Domain.Entities.Team entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/team/{id}";
    }

    private static string TeamTypeToString(TeamType teamType)
    {
        return teamType.GetStringValue();
    }

    private static int MembersToCount(List<Domain.Entities.Employee> members)
    {
        return members.Count;
    }

    private static int AssignedProjectsToCount(List<Domain.Entities.Project> projects)
    {
        return projects.Count;
    }
}