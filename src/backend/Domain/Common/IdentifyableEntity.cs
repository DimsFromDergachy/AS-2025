namespace AS_2025.Domain.Common;

public interface IIdentifiableEntity<T>
{
    T Identity { get; init; }
}