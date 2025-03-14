using System.Reflection;
using AS_2025.Common;

namespace AS_2025.ReferenceItem;

public class ReferenceEnumListBuilder
{
    public IReadOnlyDictionary<string, IReadOnlyCollection<ReferenceItem>> Build(Assembly[] assemblies)
    {
        var enums = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsEnum && Attribute.IsDefined(type, typeof(ReferenceEnumAttribute)));

        var result = new Dictionary<string, IReadOnlyCollection<ReferenceItem>>();

        foreach (var enumType in enums)
        {
            var referenceItems = new List<ReferenceItem>();

            foreach (var value in Enum.GetValues(enumType))
            {
                var name = Enum.GetName(enumType, value);
                if (name is null)
                {
                    continue;
                }

                var memberInfo = enumType.GetMember(name).FirstOrDefault();
                if (memberInfo is null)
                {
                    continue;
                }

                if (Attribute.IsDefined(memberInfo, typeof(ReferenceIgnoreAttribute)))
                {
                    continue;
                }

                if (value is not Enum enumValue)
                {
                    continue;
                }

                referenceItems.Add(new ReferenceItem(name, enumValue.GetStringValue()));
            }

            result[enumType.Name] = referenceItems;
        }

        return result.AsReadOnly();
    }
}