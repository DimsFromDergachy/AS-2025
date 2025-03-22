namespace AS_2025.Export;

public interface IXlsxDataExportService<in T>
{
    public Task<byte[]> ExportAsync(IReadOnlyCollection<T> data, string sheetName, CancellationToken cancellationToken);
}