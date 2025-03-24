namespace AS_2025.Export;

public interface IPdfDataExportService<in T>
{
    public Task<byte[]> ExportAsync(string templateName, IReadOnlyCollection<T> data, CancellationToken cancellationToken);
}