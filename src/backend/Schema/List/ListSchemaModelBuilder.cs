using System.Reflection;
using AS_2025.Common;

namespace AS_2025.Schema.List;

public class ListSchemaModelBuilder
{
    public ListSchemaModel Build<T>()
    {
        var columns = new List<ListColumnSchemaItem>();

        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<ListColumnSchemaAttribute>();
            if (attribute is null)
            {
                continue;
            }

            columns.Add(ListColumnSchemaItem.From(attribute, property.Name.ToCamelCase()));
        }

        return new ListSchemaModel
        {
            Columns = columns
        };
    }
}