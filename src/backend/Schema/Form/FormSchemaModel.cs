namespace AS_2025.Schema.Form;

public record FormSchemaModel
{
    public string Title { get; init; } = string.Empty;

    public FormInputSchemaItem[] Inputs { get; init; } = Array.Empty<FormInputSchemaItem>();
}