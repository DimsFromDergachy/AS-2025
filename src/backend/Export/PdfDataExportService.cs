using WkHtmlToPdfDotNet;

namespace AS_2025.Export;

public class PdfDataExportService<TIn> : IPdfDataExportService<TIn>
{
    private readonly ITemplateHtmlDataExportService<TIn> _templateHtmlDataExportService;

    public PdfDataExportService(ITemplateHtmlDataExportService<TIn> templateHtmlDataExportService)
    {
        _templateHtmlDataExportService = templateHtmlDataExportService;
    }

    public async Task<byte[]> ExportAsync(string templateName, IReadOnlyCollection<TIn> data, CancellationToken cancellationToken)
    {
        var htmlBytes = await _templateHtmlDataExportService.ExportAsync(templateName, data, cancellationToken);

        var converter = new SynchronizedConverter(new PdfTools());

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4Plus,
            },
            Objects = {
                new ObjectSettings {
                    PagesCount = true,
                    HtmlContent = System.Text.Encoding.UTF8.GetString(htmlBytes),
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                }
            }
        };

        return converter.Convert(doc);
    }
}