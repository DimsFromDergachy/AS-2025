using AS_2025.Import.Filesystem.Model;
using AS_2025.Import.Handlers;
using Ganss.Excel;

namespace AS_2025.Import;

public class XlsxDataImportService
{
    private readonly IDataImportHandler<Department> _handler;

    public XlsxDataImportService(IDataImportHandler<Department> handler)
    {
        _handler = handler;
    }

    public async Task Import(string filepath, int sheetIndex, CancellationToken cancellationToken)
    {
        var data = await new ExcelMapper().FetchAsync<Department>(filepath, sheetIndex);
        await _handler.HandleAsync(data, cancellationToken);
    }
}