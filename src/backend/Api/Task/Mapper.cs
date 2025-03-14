using AS_2025.Api.Task.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Task;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Task.Id), nameof(ListTasksItem.Url), Use = nameof(GuidToUrl))]
    public static partial ListTasksItem ToListItem(Domain.Entities.Task entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/task/{id}";
    }
}
