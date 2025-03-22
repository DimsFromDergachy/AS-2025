using AS_2025.Domain.Entities;

namespace AS_2025.Export.Converter;

public class DepartmentExportModelConverter : IExportModelConverter<Department, Model.Department>
{
    public IReadOnlyCollection<Model.Department> Convert(IEnumerable<Department> @in)
    {
        return @in.Select(Mapper.ToExportModel).ToList();
    }
}