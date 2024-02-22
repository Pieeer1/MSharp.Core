using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EasyDependency;
public static class StartupExtensions
{
    /// <summary>
    /// Automatically Adds Dependencies to the Service Collection
    /// </summary>
    /// <param name="serviceCollection">Service Collection to Add Dependencies to.</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void AddEasyDependencies(this IServiceCollection serviceCollection)
    {
        MethodBase? mainMethodBase = (new System.Diagnostics.StackTrace()).GetFrame(1)?.GetMethod();
        string @namespace = ParseNamespace(mainMethodBase?.DeclaringType?.Namespace ?? throw new NullReferenceException("Cannot get Startup Method"));
        Type currentType = mainMethodBase?.DeclaringType ?? throw new NullReferenceException("Cannot get Current Assembly Type");
        IEnumerable<AssemblyName> assemblies = Assembly.GetAssembly(currentType)
            ?.GetReferencedAssemblies()
            .Where(x => x.FullName.ToLower().StartsWith(@namespace)) ?? throw new NullReferenceException(@namespace);

        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is not null)
        {
            assemblies = assemblies.Append(entryAssembly.GetName());
        }

        foreach (AssemblyName assemblyName in assemblies)
        {
            Assembly assembly = Assembly.Load(assemblyName);

            IEnumerable<Type> types = assembly.GetTypes();
            foreach (Type type in types)
            {
                IEnumerable<Type> interfaces = type.GetInterfaces()
                    .Where(x => x.GetCustomAttribute<EasyDependencyAttribute>() is not null)
                    .Where(x => !x.GetTypeInfo().IsGenericTypeDefinition);
                if (type.GetCustomAttribute<EasyDependencyAttribute>() is not null && !type.IsInterface)
                {
                    AddServiceLevel(serviceCollection, type.GetCustomAttribute<EasyDependencyAttribute>()!.ServiceLifetime, null, type);
                }
                foreach (Type @interface in interfaces)
                {
                    AddServiceLevel(serviceCollection, @interface.GetCustomAttribute<EasyDependencyAttribute>()!.ServiceLifetime, @interface, type);
                }
            }
        }
    }
    private static void AddServiceLevel(IServiceCollection serviceCollection, ServiceLifetime serviceLifetime, Type? @interface, Type reference)
    {
        switch (serviceLifetime)
        {
            case ServiceLifetime.Transient:
                if (@interface is not null)
                {
                    serviceCollection.AddTransient(@interface, reference);
                }
                else
                { 
                    serviceCollection.AddTransient(reference);
                }
                break;
            case ServiceLifetime.Scoped:
                if (@interface is not null)
                {
                    serviceCollection.AddScoped(@interface, reference);
                }
                else
                {
                    serviceCollection.AddScoped(reference);
                }
                break;
            case ServiceLifetime.Singleton:
                if (@interface is not null)
                {
                    serviceCollection.AddSingleton(@interface, reference);
                }
                else
                {
                    serviceCollection.AddSingleton(reference);
                }
                break;
        }
    }

    private static string ParseNamespace(string @namespace)
    {
        return @namespace.Split('.').First().ToLower();
    }
}
