# MSharp.Core

## MSharp.Core is a simple dependency injection library for .NET 8.0+ that requires no configuration.

## Usage

### Dependency Injection

Startup
```csharp
builder.Services.AddDependencies();
```
Injections
```csharp
public class TestTransientClass : TestTransientInterface
{
}
[Dependency(ServiceLifetime.Transient)]
public interface TestTransientInterface
{
}
public class TestScopedClass : TestScopedInterface
{
}
[Dependency(ServiceLifetime.Scoped)]
public interface TestScopedInterface
{
}
public class TestSingletonClass : TestSingletonInterface
{
}
[Dependency(ServiceLifetime.Singleton)]
public interface TestSingletonInterface
{
}

```
And that is it! They will get auto injected in startup.

Automatically takes the namespace of the Startup file to make sure only YOUR dependencies get injected!


### GraphQL

This assumes you are using HotChocolate for GraphQL.

Startup
```csharp
serviceCollection.AddGraphQLServer()
.AddAutomaticGraphQLInjections();
```
Injections
```csharp

[Query]
[ExtendObjectType("Query")]
public class MyQueryClass
{
///your query class here
}
[Mutation]
[ExtendObjectType("Mutation")]
public class MyMutationClass
{
///your mutation class here
}
[Subscription]
[ExtendObjectType("Subscription")]
public class MySubscriptionClass
{
///your subscription class here
}


```