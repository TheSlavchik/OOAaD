using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.InfrastructureTests;

public class ShootingObjectAdapterTests
{
    [Fact]
    public void Position_WhenDataHasValidPosition_ReturnsVector()
    {
        var expected = new Vector(3, 5);
        var data = new Dictionary<string, object> { { "position", expected } };

        var adapter = new ShootingObjectAdapter(data);

        Assert.Same(expected, adapter.Position);
    }

    [Fact]
    public void Angle_WhenDataHasValidAngle_ReturnsAngle()
    {
        var expected = new Angle(2);
        var data = new Dictionary<string, object> { { "angle", expected } };

        var adapter = new ShootingObjectAdapter(data);

        Assert.Equal(expected, adapter.Angle);
    }

    [Fact]
    public void Position_WhenDataMissingPosition_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object>();

        var adapter = new ShootingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Position);
    }

    [Fact]
    public void Angle_WhenDataMissingAngle_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object>();

        var adapter = new ShootingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Angle);
    }

    [Fact]
    public void Position_WhenDataHasInvalidType_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object> { { "position", "not-a-vector" } };

        var adapter = new ShootingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Position);
    }

    [Fact]
    public void Angle_WhenDataHasInvalidType_ThrowsInvalidOperationException()
    {
        var data = new Dictionary<string, object> { { "angle", "not-an-angle" } };

        var adapter = new ShootingObjectAdapter(data);

        Assert.Throws<InvalidOperationException>(() => adapter.Angle);
    }

    [Fact]
    public void Position_And_Angle_ReadFromSameDictionary()
    {
        var position = new Vector(10, 20);
        var angle = new Angle(3);
        var data = new Dictionary<string, object>
        {
            { "position", position },
            { "angle", angle }
        };

        var adapter = new ShootingObjectAdapter(data);

        Assert.Same(position, adapter.Position);
        Assert.Equal(angle, adapter.Angle);
    }
}
