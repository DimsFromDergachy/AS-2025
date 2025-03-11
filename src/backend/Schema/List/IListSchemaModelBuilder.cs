namespace AS_2025.Schema.List;

public interface IListSchemaModelBuilder<in T>
{
    ListSchemaModel Build();
}