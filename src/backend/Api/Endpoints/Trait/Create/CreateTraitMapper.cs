using FastEndpoints;

namespace AS_2025.Api.Endpoints.Trait.Create;

public class CreateTraitMapper : Mapper<CreateTraitRequest, CreateTraitResponse, Domain.Trait>
{
    public override Domain.Trait ToEntity(CreateTraitRequest r)
    {
        return Domain.Trait.Create(r.Code, r.Name, r.Description);
    }

    public override CreateTraitResponse FromEntity(Domain.Trait e) => new(e.Id);
}