using System.Reflection;

namespace SpaceBattle.Lib.Infrastructure;

public static class DynamicAdapterFactory
{
    private static readonly Dictionary<Type, Dictionary<string, string?>> _propertyStrategyCache = new();

    public static T Create<T>(IDictionary<string, object> data) where T : class
    {
        var proxy = DispatchProxy.Create<T, AdapterDispatchProxy>();
        var dispatchProxy = (AdapterDispatchProxy)(object)proxy;
        dispatchProxy.Initialize(typeof(T), data);
        return proxy;
    }

    public static object Create(Type interfaceType, IDictionary<string, object> data)
    {
        var proxy = DispatchProxy.Create(interfaceType, typeof(AdapterDispatchProxy));
        var dispatchProxy = (AdapterDispatchProxy)proxy;
        dispatchProxy.Initialize(interfaceType, data);
        return proxy;
    }

    internal static string? GetPropertyStrategy(Type interfaceType, string propertyName)
    {
        lock (_propertyStrategyCache)
        {
            if (!_propertyStrategyCache.TryGetValue(interfaceType, out var props))
            {
                props = new Dictionary<string, string?>();
                foreach (var prop in interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attr = prop.GetCustomAttribute<AdapterAttribute>();
                    props[prop.Name] = attr?.StrategyKey;
                }
                _propertyStrategyCache[interfaceType] = props;
            }

            return props.GetValueOrDefault(propertyName);
        }
    }
}

internal class AdapterDispatchProxy : DispatchProxy
{
    private IDictionary<string, object>? _data;
    private Type? _interfaceType;

    public void Initialize(Type interfaceType, IDictionary<string, object> data)
    {
        _interfaceType = interfaceType;
        _data = data;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null || _data is null || _interfaceType is null)
            return null;

        if (!targetMethod.IsSpecialName)
            throw new NotSupportedException($"Method '{targetMethod.Name}' is not supported");

        if (targetMethod.Name.StartsWith("get_"))
        {
            var propName = targetMethod.Name[4..];
            var strategyKey = DynamicAdapterFactory.GetPropertyStrategy(_interfaceType, propName);

            if (strategyKey is not null)
            {
                return IoC.Resolve<object>(strategyKey, _data);
            }

            var key = Char.ToLower(propName[0]) + propName[1..];

            if (!_data.TryGetValue(key, out var val))
                throw new InvalidOperationException(
                    $"Property '{propName}' (key: '{key}') is not available in the data dictionary");

            return val;
        }

        if (targetMethod.Name.StartsWith("set_"))
        {
            var propName = targetMethod.Name[4..];
            var key = Char.ToLower(propName[0]) + propName[1..];
            _data[key] = args![0]!;
            return null;
        }

        throw new NotSupportedException($"Method '{targetMethod.Name}' is not supported");
    }
}