using AS_2025.Export.Converter;
using AS_2025.Options;
using Microsoft.Extensions.Options;
using Scriban;
using Scriban.Runtime;
using System.Text;

namespace AS_2025.Export;

public class TemplateHtmlDataExportService<TIn, TOut> : ITemplateHtmlDataExportService<TIn>
{
    private readonly IExportModelConverter<TIn, TOut> _exportModelConverter;
    private readonly IOptions<TemplatesOptions> _options;

    public TemplateHtmlDataExportService(IExportModelConverter<TIn, TOut> exportModelConverter, IOptions<TemplatesOptions> options)
    {
        _exportModelConverter = exportModelConverter;
        _options = options;
    }

    public async Task<byte[]> ExportAsync(string templateName, IReadOnlyCollection<TIn> data, CancellationToken cancellationToken)
    {
        var templatePath = Path.Combine(_options.Value.Path, $"{templateName}.htmpl");
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException(templatePath);
        }

        var templateContent = await File.ReadAllTextAsync(templatePath, cancellationToken);
        var template = Template.Parse(templateContent);

        var converted = _exportModelConverter.Convert(data);

        var render = await template.RenderAsync(new { data = converted });
        return Encoding.UTF8.GetBytes(render);
    }
}