using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Infrastructure;

public static class IoC
{
    private static readonly ConcurrentDictionary<string, Func<object[], object>> _strategies = new();

    public static void Clear()
    {
        _strategies.Clear();
    }

    public static void Register(string key, Func<object[], object> strategy)
    {
        _strategies[key] = strategy;
    }

    public static T Resolve<T>(string key, params object[] args)
    {
        if (!_strategies.TryGetValue(key, out var strategy))
        {
            throw new InvalidOperationException($"Dependency '{key}' not registered");
        }

        return (T)strategy(args);
    }
}
