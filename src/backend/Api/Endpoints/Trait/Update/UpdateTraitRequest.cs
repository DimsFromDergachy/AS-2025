namespace AS_2025.Api.Endpoints.Trait.Update;

public record UpdateTraitRequest
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}