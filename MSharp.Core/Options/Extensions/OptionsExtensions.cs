using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSharp.Core.Options.Attributes;
using System.Reflection;

namespace MSharp.Core.Options.Extensions;
public static class OptionsExtensions
{
    /// <summary>
    /// Automatically Injects Options and Secrets from KeyVault into the Configuration
    /// Domain Models with the OptionAttribute will be bound to the Configuration
    /// Domain Models with the Secret Attribute in an OptionAttribute will be bound to the Configuration and then the Secret will be injected into the property. The name of the secret is the value of the property in the configuration
    /// </summary>
    /// <param name="configuration">Configuration to Bind to</param>
    /// <param name="keyVaultUri">Uri of the Keyvault to Use</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void AddOptionsAndSecrets(this IServiceCollection serviceCollection, IConfiguration configuration, string keyVaultUri, TokenCredential? tokenCredential = null)
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        entryAssembly ??= Assembly.GetCallingAssembly();

        var optionTypes = entryAssembly.GetTypes().Where(type => type.GetCustomAttribute<OptionAttribute>() is not null);

        //if you get an exception here make sure you are authenticated (az login locally)
        SecretClient secretClient = new SecretClient(new Uri(keyVaultUri), tokenCredential ?? new DefaultAzureCredential());

        foreach (var optionType in optionTypes)
        {
            var optionAttribute = optionType.GetCustomAttribute<OptionAttribute>();

            var secretProperties = optionType.GetProperties().Where(property => property.GetCustomAttribute<SecretAttribute>() is not null);

            var configuratonSection = configuration.GetSection(optionAttribute!.ConfigurationSectionName);

            var options = configuratonSection.Get(optionType)
                ?? throw new NullReferenceException($"Could not bind section to object type: {optionType.Name}. Make sure there is a json body in your local settings named: {optionAttribute.ConfigurationSectionName}");

            foreach (var secretProperty in secretProperties)
            {
                var secretKey = configuratonSection[secretProperty.Name];

                KeyVaultSecret? secretValue = secretClient.GetSecret(secretKey);
                if (secretValue is not null)
                {
                    secretProperty.SetValue(options, secretValue.Value);
                    configuratonSection[secretProperty.Name] = secretValue.Value;
                }
            }

            MethodInfo configureInfo = typeof(OptionsServiceCollectionExtensions).GetMethods().First(x => x.Name == nameof(OptionsServiceCollectionExtensions.Configure) && x.GetParameters().Length == 2)
                ?? throw new NullReferenceException("Options Service Collection Extensions does not have a Configure Method");

            configureInfo = configureInfo.MakeGenericMethod(optionType);

            Action<object> objectAction = configuratonSection.Bind;

            object?[]? objs = [serviceCollection, objectAction];

            configureInfo.Invoke(null, objs);
        }
    }
}
