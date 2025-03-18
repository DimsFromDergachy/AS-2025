using System.Reflection;
using AS_2025.Common;

namespace AS_2025.Schema.Form;

public class FormSchemaModelBuilder
{
    public FormSchemaModel Build<T>()
    {
        var inputs = new List<FormInputSchemaItem>();

        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<FormInputSchemaAttribute>();
            if (attribute is null)
            {
                continue;
            }

            inputs.Add(FormInputSchemaItem.From(attribute, property.Name.ToCamelCase()));
        }

        var listSchemaAttribute = typeof(T).GetCustomAttribute<FormSchemaAttribute>();

        return new FormSchemaModel
        {
            Title = listSchemaAttribute?.Title ?? string.Empty,
            Inputs = inputs.ToArray()
        };
    }
}