using FastEndpoints;

namespace AS_2025.Api.Endpoints.Trait.Update;

public class UpdateTraitMapper : Mapper<UpdateTraitRequest, UpdateTraitResponse, Domain.Trait>
{
    public override Domain.Trait UpdateEntity(UpdateTraitRequest r, Domain.Trait e)
    {
        return e with
        {
            Name = r.Name,
            Description = r.Description
        };
    }

    public override UpdateTraitResponse FromEntity(Domain.Trait e) => new(e.Id, e.Code, e.Name, e.Description);
}