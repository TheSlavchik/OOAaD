using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.InfrastructureTests;

public class MovingObjectAdapterTests
{
    [Fact]
    public void Position_WhenDataHasValidPosition_ReturnsVector()
    {
        var expected = new Vector(3, 5);
        var data = new Dictionary<string, object> { { "position", expected } };

        var adapter = new MovingObjectAdapter(data);

        Assert.Same(expected, adapter.Position);
    }

    [Fact]
    public void Position_WhenDataMissingPosition_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object>();

        var adapter = new MovingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Position);
    }

    [Fact]
    public void Position_WhenDataHasInvalidType_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object> { { "position", "not-a-vector" } };

        var adapter = new MovingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Position);
    }

    [Fact]
    public void Position_Set_UpdatesValueInDictionary()
    {
        var data = new Dictionary<string, object> { { "position", new Vector(0, 0) } };
        var adapter = new MovingObjectAdapter(data);
        var newPosition = new Vector(10, 20);

        adapter.Position = newPosition;

        Assert.Same(newPosition, data["position"]);
    }

    [Fact]
    public void Position_Set_WhenKeyNotExists_AddsToDictionary()
    {
        var data = new Dictionary<string, object>();
        var adapter = new MovingObjectAdapter(data);
        var newPosition = new Vector(5, 5);

        adapter.Position = newPosition;

        Assert.Equal(newPosition, data["position"]);
    }

    [Fact]
    public void Velocity_WhenDataHasValidVelocity_ReturnsVector()
    {
        var expected = new Vector(-4, 1);
        var data = new Dictionary<string, object> { { "velocity", expected } };

        var adapter = new MovingObjectAdapter(data);

        Assert.Same(expected, adapter.Velocity);
    }

    [Fact]
    public void Velocity_WhenDataMissingVelocity_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object>();

        var adapter = new MovingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Velocity);
    }

    [Fact]
    public void Velocity_WhenDataHasInvalidType_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object> { { "velocity", "not-a-vector" } };

        var adapter = new MovingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Velocity);
    }

    [Fact]
    public void PositionAndVelocity_ReadFromSameDictionary()
    {
        var position = new Vector(10, 20);
        var velocity = new Vector(-4, 1);
        var data = new Dictionary<string, object>
        {
            { "position", position },
            { "velocity", velocity }
        };

        var adapter = new MovingObjectAdapter(data);

        Assert.Same(position, adapter.Position);
        Assert.Same(velocity, adapter.Velocity);
    }

    [Fact]
    public void Adapter_ImplementsIMovable()
    {
        var data = new Dictionary<string, object>();
        var adapter = new MovingObjectAdapter(data);

        Assert.IsAssignableFrom<IMovable>(adapter);
    }
}