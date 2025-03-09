namespace AS_2025.Domain.Common;

public interface IIdentifiableEntity<T>
{
    T ExternalId { get; init; }
}