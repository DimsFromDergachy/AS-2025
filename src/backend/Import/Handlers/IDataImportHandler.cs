namespace AS_2025.Import.Handlers;

public interface IDataImportHandler<in T>
{
    Task HandleAsync(IEnumerable<T> data, CancellationToken  cancellationToken);
}