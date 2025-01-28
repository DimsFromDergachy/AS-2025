using AS_2025.Domain.Common;

namespace AS_2025.Domain.Entities;

public record Trait : Entity<Guid>
{
    public sealed override Guid Id { get; init; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    private Trait(Guid Id)
    {
        this.Id = Id;
    }

    public Trait(string code, string name, string description)
    {
        Id = Guid.CreateVersion7();
        Code = code;
        Name = name;
        Description = description;
    }

    public static Trait Create(string code, string name, string description)
    {
        return new Trait(Guid.CreateVersion7())
        {
            Code = code,
            Name = name,
            Description = description
        };
    }
}