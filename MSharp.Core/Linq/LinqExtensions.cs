namespace MSharp.Core.Linq;
public static class LinqExtensions
{
    /// <summary>
    /// Should be a Default Feature of C#, function that converts an enumerable with null values to an enumerable with none
    /// </summary>
    /// <typeparam name="T">Inside of the Enumerable Type</typeparam>
    /// <param name="obj">Enumerable of Type T</param>
    /// <returns>Same object with no null values</returns>
    public static IEnumerable<T> ToNonNullableInside<T>(this IEnumerable<T?> obj) where T : class
    {
        foreach (T? t in obj)
        {
            if (t is not null)
            {
                yield return t;
            }
        }
    }
    /// <summary>
    /// Should be a Default Feature of C#, function that converts an enumerable with null values to an enumerable with none
    /// </summary>
    /// <typeparam name="T">Inside of the Enumerable Type</typeparam>
    /// <param name="obj">Enumerable of Type T</param>
    /// <returns>Same object with no null values</returns>
    public static IEnumerable<T> ToNonNullableInside<T>(this IEnumerable<T?> obj) where T : struct
    {
        foreach (T? t in obj)
        {
            if (t is not null)
            {
                yield return (T)t;
            }
        }
    }
}
