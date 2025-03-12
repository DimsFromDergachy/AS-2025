namespace AS_2025.ReferenceItem;

public class ReferenceListBuilder
{
    public IReadOnlyCollection<ReferenceItem> Build<T>(List<T> list)
    {
        var properties = typeof(T).GetProperties();
        var keyProperty = properties.FirstOrDefault(x => Attribute.IsDefined(x, typeof(ReferenceKeyAttribute)));
        var valueProperty = properties.FirstOrDefault(x => Attribute.IsDefined(x, typeof(ReferenceValueAttribute)));

        if (keyProperty is null || valueProperty is null)
        {
            return Array.Empty<ReferenceItem>();
        }

        return list.Select(x => new ReferenceItem(
            Convert.ToString(keyProperty.GetValue(x) ?? string.Empty) ?? string.Empty,
            Convert.ToString(valueProperty.GetValue(x) ?? string.Empty) ?? string.Empty
        )).Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value)).ToList();
    }
}