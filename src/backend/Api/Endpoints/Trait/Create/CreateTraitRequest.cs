namespace AS_2025.Api.Endpoints.Trait.Create;

public record CreateTraitRequest
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}