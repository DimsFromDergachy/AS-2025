
using AS_2025.Api.Employee.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Employee;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Employee.Id), nameof(ListEmployeesItem.Url), Use = nameof(GuidToUrl))]
    public static partial ListEmployeesItem ToListItem(Domain.Entities.Employee entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/employee/{id}";
    }
}
