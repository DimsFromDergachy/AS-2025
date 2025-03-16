using AS_2025.Common;
using AS_2025.Domain.Common;
using AS_2025.Import.Filesystem.Model;

namespace AS_2025.Import.Handlers;

public class TeamDataImportHandler : BaseDataImportHandler, IDataImportHandler<Team>
{
    public TeamDataImportHandler(IContext context, ImportDataContext importDataContext) : base(context, importDataContext)
    {
    }

    public async Task HandleAsync(List<Team> data, CancellationToken cancellationToken)
    {
        ImportDataContext.WithTeams(data);

        var teamsImported = data.Select(x => new Domain.Entities.Team()
        {
            ExternalId = x.Id,
            Name = x.Name,
            Type = Enum.TryParse(typeof(TeamType), x.Type, out var type) ? (TeamType)type : TeamType.Undefined,
            Department = TryGetDepartment(x.DepartmentId, out var department) ? department : null,
            TeamLead = TryGetEmployee(x.TeamLeadId, out var teamLead) ? teamLead : null,
        }).ToList();

        foreach (var team in teamsImported)
        {
            if (!TryGetTeam(team.ExternalId, out var existing))
            {
                Context.Teams.Add(team);
                continue;
            }

            existing.Update(team);
        }

        await Context.SaveChangesAsync(cancellationToken);
    }
}