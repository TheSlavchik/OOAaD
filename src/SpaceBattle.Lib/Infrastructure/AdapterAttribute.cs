using System.Reflection;

namespace SpaceBattle.Lib.Infrastructure;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class AdapterAttribute : Attribute
{
    public Type InterfaceType { get; }
    public string PropertyName { get; }
    public string StrategyKey { get; }

    public AdapterAttribute(Type interfaceType, string propertyName, string strategyKey)
    {
        InterfaceType = interfaceType;
        PropertyName = propertyName;
        StrategyKey = strategyKey;
    }

    public static string? GetPropertyStrategyKey(Type interfaceType, string propertyName)
    {
        var prop = interfaceType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop is null) return null;

        var attr = prop.GetCustomAttribute<AdapterAttribute>();
        return attr?.StrategyKey;
    }
}