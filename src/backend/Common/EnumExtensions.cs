using System.Reflection;

namespace AS_2025.Common;

public static class EnumExtensions
{
    public static string GetStringValue(this Enum value)
    {
        var type = value.GetType();
        var fieldInfo = type.GetField(value.ToString());
        return fieldInfo?.GetCustomAttribute(typeof(StringValueAttribute), false) is StringValueAttribute attribute 
            ? attribute.Value 
            : value.ToString();
    }
}