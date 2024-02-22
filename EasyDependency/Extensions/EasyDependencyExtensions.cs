using System.Reflection;
using MSharp.Core.Linq;
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