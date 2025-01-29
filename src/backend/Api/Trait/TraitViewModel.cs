namespace AS_2025.Api.Trait;

public record TraitViewModel
{
    public Guid Id { get; init; }

    public string Code { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }
}