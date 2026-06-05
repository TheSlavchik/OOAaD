using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Infrastructure;

public class GameObjectRepository : IGameObjectRepository
{
    private readonly Dictionary<Guid, IDictionary<string, object>> _storage = new();

    public void Add(Guid id, IDictionary<string, object> obj)
    {
        _storage[id] = obj;
    }

    public void Remove(Guid id)
    {
        _storage.Remove(id);
    }

    public IDictionary<string, object>? GetById(Guid id)
    {
        return _storage.GetValueOrDefault(id);
    }
}
