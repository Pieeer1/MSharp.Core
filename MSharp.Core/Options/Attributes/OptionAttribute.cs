namespace MSharp.Core.Options.Attributes;
[AttributeUsage(AttributeTargets.Class)]
public class OptionAttribute(string configurationSectionName) : Attribute
{
    public string ConfigurationSectionName { get; } = configurationSectionName;
}
