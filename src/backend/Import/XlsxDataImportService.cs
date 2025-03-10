using AS_2025.Import.Handlers;
using Ganss.Excel;

namespace AS_2025.Import;

public class XlsxDataImportService<T>
{
    private readonly IDataImportHandler<T> _handler;

    public XlsxDataImportService(IDataImportHandler<T> handler)
    {
        _handler = handler;
    }

    public async Task Import(string filepath, int sheetIndex, CancellationToken cancellationToken)
    {
        var data = await new ExcelMapper().FetchAsync<T>(filepath, sheetIndex);
        await _handler.HandleAsync(data.ToList(), cancellationToken);
    }
}