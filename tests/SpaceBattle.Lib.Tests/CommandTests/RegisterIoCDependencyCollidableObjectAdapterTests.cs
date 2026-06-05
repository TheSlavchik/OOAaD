using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencyCollidableObjectAdapterTests
{
    public RegisterIoCDependencyCollidableObjectAdapterTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IoC.Resolve<ICollidable>("Adapters.ICollidableObject", new Dictionary<string, object>()));
    }

    [Fact]
    public void Execute_RegistersDependency_AndCollidableObjectAdapterResolves()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) },
            { "radius", 3 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.NotNull(collidable);
        Assert.IsType<CollidableObjectAdapter>(collidable);
        Assert.Equal(obj["position"], collidable.Position);
        Assert.Equal(obj["radius"], collidable.Radius);
    }

    [Fact]
    public void Execute_ResolvedAdapter_ReadsPositionAndRadius()
    {
        var position = new Vector(10, 20);
        var radius = 5;

        var obj = new Dictionary<string, object>
        {
            { "position", position },
            { "radius", radius }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.Equal(position, collidable.Position);
        Assert.Equal(radius, collidable.Radius);
    }
}
