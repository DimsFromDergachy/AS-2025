using Ardalis.Result;
using MediatR;
using System.Text.Json.Serialization;

namespace AS_2025.Api.Trait.Update;

public record UpdateTraitRequest : IRequest<Result<TraitViewModel>>
{
    [JsonIgnore]
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}