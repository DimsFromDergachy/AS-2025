namespace AS_2025.Import;

public interface IXlsxDataImportService
{
    Task Import(string filepath, int sheetIndex, CancellationToken cancellationToken);
}