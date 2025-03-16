namespace AS_2025.Import;

public interface IJsonDataImportService
{
    Task Import(string filepath, CancellationToken cancellationToken);
}