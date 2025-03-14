namespace AS_2025.Import.Handlers;

public interface IDataImportHandler<T>
{
    Task HandleAsync(List<T> data, CancellationToken  cancellationToken);
}