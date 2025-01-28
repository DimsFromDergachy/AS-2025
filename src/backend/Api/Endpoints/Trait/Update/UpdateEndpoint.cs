using AS_2025.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace AS_2025.Api.Endpoints.Trait.Update;

[HttpPut("/trait/{id}")]
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
public class UpdateEndpoint : Endpoint<UpdateTraitRequest, UpdateTraitResponse, UpdateTraitMapper>
{
    private readonly ITraitRepository _traitRepository;

    public UpdateEndpoint(ITraitRepository traitRepository)
    {
        _traitRepository = traitRepository;
    }

    public override async Task HandleAsync(UpdateTraitRequest req, CancellationToken ct)
    {
        var id = Route<string>("id");
        if (string.IsNullOrWhiteSpace(id))
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var trait = await _traitRepository.GetAsync(Guid.Parse(id), ct);
        if (trait is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var updated = Map.UpdateEntity(req, trait);
        await _traitRepository.SaveAsync(updated, ct);

        Response = Map.FromEntity(updated);
        await SendAsync(Response, cancellation: ct);
    }
}