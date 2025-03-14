using AS_2025.Api.Project.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Project;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Project.Id), nameof(ListProjectsItem.Url), Use = nameof(GuidToUrl))]
    public static partial ListProjectsItem ToListItem(Domain.Entities.Project entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/project/{id}";
    }
}
