# EasyDependency

## Easy Dependency is a simple dependency injection library for .NET 8.0+ that requires no configuration.

### Usage


Startup
```csharp
builder.Services.AddEasyDependencies();
```
Injections
```csharp
public class TestTransientClass : TestTransientInterface
{
}
[EasyDependency(ServiceLifetime.Transient)]
public interface TestTransientInterface
{
}
public class TestScopedClass : TestScopedInterface
{
}
[EasyDependency(ServiceLifetime.Scoped)]
public interface TestScopedInterface
{
}
public class TestSingletonClass : TestSingletonInterface
{
}
[EasyDependency(ServiceLifetime.Singleton)]
public interface TestSingletonInterface
{
}

```
And that is it! They will get auto injected in startup.

Automatically takes the namespace of the Startup file to make sure only YOUR dependencies get injected!
