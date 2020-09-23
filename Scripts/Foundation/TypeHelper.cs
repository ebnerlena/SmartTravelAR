using System;

public class TypeHelper
{
    public static string ToString(Type resourceType)
    {
        return resourceType.FullName;
    }
    public static bool TryParseType<T>(string resourceTypeFullName, out Type type)
    {
        type = Type.GetType(resourceTypeFullName);
        return typeof(T).IsAssignableFrom(type);
    }

    public static bool IsTypeOf<T>(Type typeToCheck)
    {
        return typeof(T).IsAssignableFrom(typeToCheck);
    }
}