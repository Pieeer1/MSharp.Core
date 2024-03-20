using MSharp.Core.Linq;
namespace MSharp.Core.Tests;
public class LinqExtensionsTests
{
    [Fact]
    public void Test_NonNullableInside_ReferenceType()
    {
        IEnumerable<string?> nullableList = new List<string?> { "Hello", null, "World" };
        IEnumerable<string> nonNullableList = nullableList.ToNonNullableInside();
        Assert.Equal(2, nonNullableList.Count());
    }
    [Fact]
    public void Test_NonNullableInside_ValueType()
    {
        IEnumerable<int?> nullableList = new List<int?> { 1, null, 3 };
        IEnumerable<int> nonNullableList = nullableList.ToNonNullableInside();
        Assert.Equal(2, nonNullableList.Count());
    }
}
