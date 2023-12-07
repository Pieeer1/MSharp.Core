using Microsoft.Extensions.DependencyInjection;

namespace EasyDependency.Tests;

public class StartupExtensionTests
{
    [Fact]
    public void ValidateAdditions()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddEasyDependencies();

        Assert.Equal(3, serviceCollection.Count);

        Assert.Equal("TestTransientInterface", serviceCollection.ElementAt(0).ServiceType.Name);
        Assert.Equal("TestTransientClass", serviceCollection.ElementAt(0).ImplementationType?.Name);
        Assert.Equal(ServiceLifetime.Transient, serviceCollection.ElementAt(0).Lifetime);
        Assert.Equal("TestScopedInterface", serviceCollection.ElementAt(1).ServiceType.Name);
        Assert.Equal("TestScopedClass", serviceCollection.ElementAt(1).ImplementationType?.Name);
        Assert.Equal(ServiceLifetime.Scoped, serviceCollection.ElementAt(1).Lifetime);
        Assert.Equal("TestSingletonInterface", serviceCollection.ElementAt(2).ServiceType.Name);
        Assert.Equal("TestSingletonClass", serviceCollection.ElementAt(2).ImplementationType?.Name);
        Assert.Equal(ServiceLifetime.Singleton, serviceCollection.ElementAt(2).Lifetime);


    }
}