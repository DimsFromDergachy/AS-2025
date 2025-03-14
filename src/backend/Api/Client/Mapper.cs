using AS_2025.Api.Client.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Client;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Client.Id), nameof(ListClientsItem.Url), Use = nameof(GuidToUrl))]
    public static partial ListClientsItem ToListItem(Domain.Entities.Client entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/client/{id}";
    }
}
