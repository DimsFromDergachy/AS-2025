using AS_2025.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace AS_2025.Api.Endpoints.Trait.Create;

[HttpPost("/trait")]
[AllowAnonymous]
/*
 * [Http{VERB}("/route")] - sets up the verb and route
 * [AllowAnonymous] - allows un-authenticated access
 * [AllowFileUploads] - allows file uploads with multipart/form-data
 * [Authorize(...)] - specifies authorization requirements with roles and policies
 * [Group<TGroup>] - associates an endpoint with a configuration group
 * [PreProcessor<TProcessor>] - adds a pre-processor to the pipeline
 * [PostProcessor<TProcessor>] - adds a post-processor to the pipeline
 */
public class CreateEndpoint : Endpoint<CreateTraitRequest, CreateTraitResponse, CreateTraitMapper>
{
    private readonly ITraitRepository _traitRepository;

    public CreateEndpoint(ITraitRepository traitRepository)
    {
        _traitRepository = traitRepository;
    }

    public override async Task HandleAsync(CreateTraitRequest req, CancellationToken ct)
    {
        var trait = Map.ToEntity(req);
        await _traitRepository.SaveAsync(trait, ct);

        Response = Map.FromEntity(trait);
        await SendAsync(Response, cancellation: ct);
    }
}