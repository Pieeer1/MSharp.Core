using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EasyDependency;
public static class StartupExtensions
{
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
                foreach (Type @interface in interfaces)
                {
                    switch (@interface.GetCustomAttribute<EasyDependencyAttribute>()?.ServiceLifetime)
                    {
                        case ServiceLifetime.Transient:
                            serviceCollection.AddTransient(@interface, type);
                            break;
                        case ServiceLifetime.Scoped:
                            serviceCollection.AddScoped(@interface, type);
                            break;
                        case ServiceLifetime.Singleton:
                            serviceCollection.AddSingleton(@interface, type);
                            break;
                    }
                }
            }
        }
    }

    private static string ParseNamespace(string @namespace)
    {
        return @namespace.Split('.').First().ToLower();
    }
}
