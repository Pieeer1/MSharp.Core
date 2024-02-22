using Microsoft.Extensions.DependencyInjection;

namespace MSharp.Core.Dependency;
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class DependencyAttribute : Attribute
{
    public DependencyAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }

    public ServiceLifetime ServiceLifetime { get; private set; }
}

