using Microsoft.Extensions.DependencyInjection;

namespace EasyDependency;
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class EasyDependencyAttribute : Attribute
{
    public EasyDependencyAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }

    public ServiceLifetime ServiceLifetime { get; private set; }
}

