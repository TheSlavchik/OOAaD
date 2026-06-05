using Moq;
using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Tests.AbstractionTests;

public class GameObjectRepositoryTests
{
    private readonly Mock<IGameObjectRepository> _mock = new();

    [Fact]
    public void Add_ValidIdAndObject_AddsSuccessfully()
    {
        var id = Guid.NewGuid();
        var obj = new Dictionary<string, object> { { "key", "value" } };

        _mock.Object.Add(id, obj);

        _mock.Verify(r => r.Add(id, obj), Times.Once);
    }

    [Fact]
    public void Remove_ExistingId_RemovesSuccessfully()
    {
        var id = Guid.NewGuid();

        _mock.Object.Remove(id);

        _mock.Verify(r => r.Remove(id), Times.Once);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsObject()
    {
        var id = Guid.NewGuid();
        var expected = new Dictionary<string, object> { { "x", 10.0 } };

        _mock.Setup(r => r.GetById(id)).Returns(expected);

        var result = _mock.Object.GetById(id);

        Assert.Same(expected, result);
        _mock.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public void GetById_NonExistentId_ReturnsNull()
    {
        var id = Guid.NewGuid();

        _mock.Setup(r => r.GetById(id)).Returns((IDictionary<string, object>?)null);

        var result = _mock.Object.GetById(id);

        Assert.Null(result);
        _mock.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public void Add_SameIdTwice_ReplacesPreviousObject()
    {
        var id = Guid.NewGuid();
        var first = new Dictionary<string, object> { { "a", 1 } };
        var second = new Dictionary<string, object> { { "b", 2 } };

        _mock.Object.Add(id, first);
        _mock.Object.Add(id, second);

        _mock.Setup(r => r.GetById(id)).Returns(second);

        var result = _mock.Object.GetById(id);

        Assert.Same(second, result);
        _mock.Verify(r => r.Add(id, first), Times.Once);
        _mock.Verify(r => r.Add(id, second), Times.Once);
    }

    [Fact]
    public void Add_EmptyDictionary_AddsSuccessfully()
    {
        var id = Guid.NewGuid();
        var empty = new Dictionary<string, object>();

        _mock.Object.Add(id, empty);

        _mock.Verify(r => r.Add(id, empty), Times.Once);
    }

    [Fact]
    public void Add_NullDictionary_ThrowsArgumentNullException()
    {
        var id = Guid.NewGuid();

        _mock.Setup(r => r.Add(id, null!)).Throws<ArgumentNullException>();

        Assert.Throws<ArgumentNullException>(() => _mock.Object.Add(id, null!));
    }

    [Fact]
    public void Remove_NonExistentId_DoesNotThrow()
    {
        var id = Guid.NewGuid();

        _mock.Setup(r => r.Remove(id)).Callback(() => { });

        var exception = Record.Exception(() => _mock.Object.Remove(id));

        Assert.Null(exception);
        _mock.Verify(r => r.Remove(id), Times.Once);
    }

    [Fact]
    public void Remove_EmptyGuid_RemovesSuccessfully()
    {
        var id = Guid.Empty;

        _mock.Object.Remove(id);

        _mock.Verify(r => r.Remove(id), Times.Once);
    }

    [Fact]
    public void Add_EmptyGuid_AddsSuccessfully()
    {
        var id = Guid.Empty;
        var obj = new Dictionary<string, object> { { "key", "value" } };

        _mock.Object.Add(id, obj);

        _mock.Verify(r => r.Add(id, obj), Times.Once);
    }
}
