using Microsoft.Extensions.DependencyInjection;

namespace EasyDependency.Examples;
internal class TestTransientClass : TestTransientInterface
{
    internal TestTransientClass()
    {
    }
}
[EasyDependency(ServiceLifetime.Transient)]
internal interface TestTransientInterface
{
}
internal class TestScopedClass : TestScopedInterface
{
    internal TestScopedClass()
    {
    }
}
[EasyDependency(ServiceLifetime.Scoped)]
internal interface TestScopedInterface
{
}
internal class TestSingletonClass : TestSingletonInterface
{
    internal TestSingletonClass()
    {
    }
}
[EasyDependency(ServiceLifetime.Singleton)]
internal interface TestSingletonInterface
{
}
