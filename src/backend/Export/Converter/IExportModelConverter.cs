namespace AS_2025.Export.Converter;

public interface IExportModelConverter<in TIn, out TOut>
{
    public IReadOnlyCollection<TOut> Convert(IEnumerable<TIn> @in);
}