using AS_2025.Api.Department.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Department;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Department.Id), nameof(ListDepartmentsItem.Url), Use = nameof(GuidToUrl))]
    public static partial ListDepartmentsItem ToListItem(Domain.Entities.Department entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/department/{id}";
    }
}
