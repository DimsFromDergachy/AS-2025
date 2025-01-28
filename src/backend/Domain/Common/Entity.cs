namespace AS_2025.Domain.Common;

public abstract record Entity<T>
{
    public virtual T Id { get; init; } = default!;
}