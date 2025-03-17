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

        var listSchemaAttribute = typeof(T).GetCustomAttribute<ListSchemaAttribute>();

        return new ListSchemaModel
        {
            Title = listSchemaAttribute?.Title ?? string.Empty,
            Columns = columns.ToArray(),
            ColumnActions = listSchemaAttribute?.ColumnActions ?? new [] { ListAction.None },
            CommonActions = listSchemaAttribute?.CommonActions ?? new [] { ListAction.None }
        };
    }
}