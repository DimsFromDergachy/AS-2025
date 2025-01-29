using AS_2025.Api.Trait.List;
using AS_2025.Common;

namespace AS_2025.Schema.List;

public interface IListSchemaModelBuilder<T>
{
    ListSchemaModel Build();
}

public class ListTraitsSchemaModelBuilder : IListSchemaModelBuilder<ListTraitsItem>
{
    public ListSchemaModel Build()
    {
        return new ListSchemaModel
        {
            Columns = new List<ListSchemaColumnModel>
            {
                new ListSchemaColumnModel()
                {
                    Key = nameof(ListTraitsItem.Id).ToCamelCase()
                },
                new ListSchemaColumnModel()
                {
                    Key = nameof(ListTraitsItem.Code).ToCamelCase(),
                    Order = 1,
                    IsVisible = true,
                    IsVisibleByDefault = true,
                    FieldType = FieldType.String,
                    FieldDisplayType = FieldDisplayType.Link
                },
                new ListSchemaColumnModel()
                {
                    Key = nameof(ListTraitsItem.Name).ToCamelCase(),
                    Order = 2,
                    IsVisible = true,
                    IsVisibleByDefault = true,
                    FieldType = FieldType.String,
                    FieldDisplayType = FieldDisplayType.String
                }
            }
        };
    }
}