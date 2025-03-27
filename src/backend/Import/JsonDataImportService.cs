using System.Text.Json;
using AS_2025.Import.Handlers;

namespace AS_2025.Import;

public class JsonDataImportService<T> : IJsonDataImportService
{
    private readonly IDataImportHandler<T> _handler;

    public JsonDataImportService(IDataImportHandler<T> handler)
    {
        _handler = handler;
    }

    public async Task Import(string filepath, CancellationToken cancellationToken)
    {
        await using var openStream = File.OpenRead(filepath);
        var data = await JsonSerializer.DeserializeAsync<List<T>>(openStream, JsonSerializerOptions.Default, cancellationToken);
        await _handler.HandleAsync(data is not null ? data.ToList() : [], cancellationToken);
    }
}