namespace AS_2025.Api.Trait.List;

public record ListTraitsItem
{
    public Guid Id { get; init; }
    
    public string Url { get; init; }
    
    public string Code { get; init; }
    
    public string Name { get; init; }
}