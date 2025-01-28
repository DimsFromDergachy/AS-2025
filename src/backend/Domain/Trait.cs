namespace AS_2025.Domain;

public record Trait
{
    public Guid Id { get; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    private Trait(Guid Id)
    {
        this.Id = Id;
    }

    public static Trait Create(string code, string name, string description)
    {
        return new Trait(Guid.NewGuid())
        {
            Code = code,
            Name = name,
            Description = description
        };
    }
}