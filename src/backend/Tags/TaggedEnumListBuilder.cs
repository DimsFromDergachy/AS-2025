using System.Reflection;
using AS_2025.Common;

namespace AS_2025.Tags;

public class TaggedEnumListBuilder
{
    public IReadOnlyDictionary<string, IReadOnlyCollection<TaggedEnumItem>> Build(Assembly[] assemblies)
    {
        var enums = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsEnum && Attribute.IsDefined(type, typeof(TaggedEnumAttribute)));

        var result = new Dictionary<string, IReadOnlyCollection<TaggedEnumItem>>();

        foreach (var enumType in enums)
        {
            var referenceItems = new List<TaggedEnumItem>();

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

                if (value is not Enum enumValue)
                {
                    continue;
                }

                var tagStyleAttribute = memberInfo.GetCustomAttribute<TagStyleAttribute>();
                if (tagStyleAttribute is null)
                {
                    continue;
                }

                referenceItems.Add(new TaggedEnumItem(name, enumValue.GetStringValue(), tagStyleAttribute.Style.GetStringValue()));
            }

            result[enumType.Name] = referenceItems;
        }

        return result.AsReadOnly();
    }
}