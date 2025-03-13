var templates = new Dictionary<string, string>();

templates["Domain/Entities/{model}.cs"] = @"
using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AS_2025.Domain.Entities;

public record {{ model_name }} : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;
}
";

