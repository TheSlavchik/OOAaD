using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.InfrastructureTests;

public class GameObjectRepositoryConcreteTests
{
    private readonly GameObjectRepository _repository = new();

    [Fact]
    public void Add_ValidIdAndObject_StoresAndReturnsObject()
    {
        var id = Guid.NewGuid();
        var obj = new Dictionary<string, object> { { "position", new { x = 1, y = 2 } } };

        _repository.Add(id, obj);
        var result = _repository.GetById(id);

        Assert.Same(obj, result);
    }

    [Fact]
    public void GetById_NonExistentId_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = _repository.GetById(id);

        Assert.Null(result);
    }

    [Fact]
    public void Remove_ExistingId_ObjectNoLongerAvailable()
    {
        var id = Guid.NewGuid();
        _repository.Add(id, new Dictionary<string, object> { { "key", "value" } });

        _repository.Remove(id);
        var result = _repository.GetById(id);

        Assert.Null(result);
    }

    [Fact]
    public void Remove_NonExistentId_DoesNotThrow()
    {
        var id = Guid.NewGuid();

        var exception = Record.Exception(() => _repository.Remove(id));

        Assert.Null(exception);
    }

    [Fact]
    public void Add_SameIdTwice_ReplacesObject()
    {
        var id = Guid.NewGuid();
        var first = new Dictionary<string, object> { { "data", 1 } };
        var second = new Dictionary<string, object> { { "data", 2 } };

        _repository.Add(id, first);
        _repository.Add(id, second);
        var result = _repository.GetById(id);

        Assert.Same(second, result);
    }

    [Fact]
    public void Add_EmptyDictionary_StoresSuccessfully()
    {
        var id = Guid.NewGuid();
        var empty = new Dictionary<string, object>();

        _repository.Add(id, empty);
        var result = _repository.GetById(id);

        Assert.Same(empty, result);
    }

    [Fact]
    public void GetById_AfterMultipleAdds_ReturnsCorrectObject()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var obj1 = new Dictionary<string, object> { { "a", 1 } };
        var obj2 = new Dictionary<string, object> { { "b", 2 } };

        _repository.Add(id1, obj1);
        _repository.Add(id2, obj2);

        Assert.Same(obj1, _repository.GetById(id1));
        Assert.Same(obj2, _repository.GetById(id2));
    }

    [Fact]
    public void Remove_ThenAddSameId_StoresNewObject()
    {
        var id = Guid.NewGuid();
        var first = new Dictionary<string, object> { { "value", "first" } };
        var second = new Dictionary<string, object> { { "value", "second" } };

        _repository.Add(id, first);
        _repository.Remove(id);
        _repository.Add(id, second);

        var result = _repository.GetById(id);
        Assert.Same(second, result);
    }
}
