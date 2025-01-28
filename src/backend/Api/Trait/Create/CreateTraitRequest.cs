using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Trait.Create;

public record CreateTraitRequest : IRequest<Result<TraitViewModel>>
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}