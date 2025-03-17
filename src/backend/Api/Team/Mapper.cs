using AS_2025.Api.Team.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Team;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Team.Id), nameof(ListTeamsItem.Url), Use = nameof(GuidToUrl))]
    public static partial ListTeamsItem ToListItem(Domain.Entities.Team entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/team/{id}";
    }
}
