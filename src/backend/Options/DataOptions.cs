using AS_2025.Import;

namespace AS_2025.Options;

public record DataOptions
{
    public string Root { get; init; } = "./data/2025";

    public ImportDataType DataType { get; init; } = ImportDataType.Json;
}