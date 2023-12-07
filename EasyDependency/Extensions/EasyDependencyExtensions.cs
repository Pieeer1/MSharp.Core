using System.Reflection;

namespace EasyDependency.Extensions;
internal static class TypeLoaderExtensions
{
    public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null).ToNonNullableInside();
        }
    }
}
internal static class EasyDependencyExtensions
{
    public static IEnumerable<T> ToNonNullableInside<T>(this IEnumerable<T?> obj) where T : class
    {
        foreach (var o in obj)
        {
            if (o is not null)
            {
                yield return o;
            }
        }
    }
    public static IEnumerable<T> ToNonNullableInside<T>(this IEnumerable<T?> obj) where T : struct
    {
        foreach (var o in obj)
        {
            if (o is not null)
            {
                yield return (T)o;
            }
        }
    }
}
