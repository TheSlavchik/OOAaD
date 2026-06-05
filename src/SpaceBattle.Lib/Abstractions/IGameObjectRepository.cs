namespace SpaceBattle.Lib.Abstractions;

public interface IGameObjectRepository
{
    void Add(Guid id, IDictionary<string, object> obj);
    void Remove(Guid id);
    IDictionary<string, object>? GetById(Guid id);
}
