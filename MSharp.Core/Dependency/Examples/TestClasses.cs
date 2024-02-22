using Microsoft.Extensions.DependencyInjection;

namespace MSharp.Core.Dependency.Examples;
internal class TestTransientClass : TestTransientInterface
{
    internal TestTransientClass()
    {
    }
}
[Dependency(ServiceLifetime.Transient)]
internal interface TestTransientInterface
{
}
internal class TestScopedClass : TestScopedInterface
{
    internal TestScopedClass()
    {
    }
}
[Dependency(ServiceLifetime.Scoped)]
internal interface TestScopedInterface
{
}
internal class TestSingletonClass : TestSingletonInterface
{
    internal TestSingletonClass()
    {
    }
}
[Dependency(ServiceLifetime.Singleton)]
internal interface TestSingletonInterface
{
}
[Dependency(ServiceLifetime.Scoped)]
internal class TestScopedClassRawDependency
{ 

}
