using AS_2025.Api.Trait.Create;
using AS_2025.Api.Trait.List;
using AS_2025.Api.Trait.Update;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Trait;

[Mapper]
public static partial class Mapper
{
    public static partial Domain.Entities.Trait ToTraitEntity(CreateTraitRequest request);

    public static partial void UpdateEntity([MappingTarget] this Domain.Entities.Trait entity, UpdateTraitRequest request);

    public static partial TraitViewModel ToTraitViewModel(Domain.Entities.Trait entity);

    [MapProperty(nameof(Domain.Entities.Trait.Id), nameof(ListTraitsItem.Url))]
    public static partial ListTraitsItem ToListTraitsItem(Domain.Entities.Trait entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/trait/{id}";
    }
}