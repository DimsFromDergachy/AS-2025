using AS_2025.Export.Converter;
using Ganss.Excel;

namespace AS_2025.Export;

public class XlsxDataExportService<TIn, TOut> : IXlsxDataExportService<TIn>
{
    private readonly IExportModelConverter<TIn, TOut> _exportModelConverter;

    public XlsxDataExportService(IExportModelConverter<TIn, TOut> exportModelConverter)
    {
        _exportModelConverter = exportModelConverter;
    }

    public async Task<byte[]> ExportAsync(IReadOnlyCollection<TIn> data, string sheetName, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();

        var converted = _exportModelConverter.Convert(data);
        await new ExcelMapper().SaveAsync(stream, converted, sheetName);

        return stream.ToArray();
    }
}