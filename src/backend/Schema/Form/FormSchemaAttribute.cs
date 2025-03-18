namespace AS_2025.Schema.Form;

[AttributeUsage(AttributeTargets.Class)]
public class FormSchemaAttribute : Attribute
{
    public string Title { get; init; } = string.Empty;
}