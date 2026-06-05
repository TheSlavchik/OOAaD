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

    [Fact]
    public void Execute_ResolvedAdapter_WhenPositionIsMissing_ThrowsInvalidOperationException()
    {
        var obj = new Dictionary<string, object>
        {
            { "radius", 3 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.Throws<InvalidOperationException>(() => collidable.Position);
    }

    [Fact]
    public void Execute_ResolvedAdapter_WhenRadiusIsMissing_ThrowsInvalidOperationException()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.Throws<InvalidOperationException>(() => collidable.Radius);
    }

    [Fact]
    public void Execute_ResolvedAdapter_WhenPositionHasWrongType_ThrowsInvalidOperationException()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", "not a vector" },
            { "radius", 3 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.Throws<InvalidOperationException>(() => collidable.Position);
    }

    [Fact]
    public void Execute_ResolvedAdapter_WhenRadiusHasWrongType_ThrowsInvalidOperationException()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) },
            { "radius", "not an int" }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.Throws<InvalidOperationException>(() => collidable.Radius);
    }

    [Fact]
    public void Execute_RegisterCommand_ExecutesWithoutException()
    {
        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();

        var exception = Record.Exception(() => registerCommand.Execute());

        Assert.Null(exception);
    }

    [Fact]
    public void Execute_ResolvedAdapter_DifferentObjectsHaveDifferentPositions()
    {
        var obj1 = new Dictionary<string, object>
        {
            { "position", new Vector(0, 0) },
            { "radius", 1 }
        };
        var obj2 = new Dictionary<string, object>
        {
            { "position", new Vector(10, 10) },
            { "radius", 5 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable1 = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj1);
        var collidable2 = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj2);

        Assert.NotEqual(collidable1.Position, collidable2.Position);
        Assert.NotEqual(collidable1.Radius, collidable2.Radius);
    }

    [Fact]
    public void Execute_ResolvedAdapter_ReturnsCorrectRadiusType()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(0, 0) },
            { "radius", 7 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.IsType<int>(collidable.Radius);
    }

    [Fact]
    public void Execute_ResolvedAdapter_ZeroRadiusIsValid()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(0, 0) },
            { "radius", 0 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", obj);

        Assert.Equal(0, collidable.Radius);
    }

    [Fact]
    public void Execute_ResolvedAdapter_AdapterStateIsIndependentBetweenInstances()
    {
        var sharedDict = new Dictionary<string, object>
        {
            { "position", new Vector(1, 2) },
            { "radius", 3 }
        };

        var registerCommand = new RegisterIoCDependencyCollidableObjectAdapter();
        registerCommand.Execute();

        var collidable1 = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", sharedDict);
        var collidable2 = IoC.Resolve<ICollidable>("Adapters.ICollidableObject", sharedDict);

        Assert.Equal(collidable1.Position, collidable2.Position);
        Assert.Equal(collidable1.Radius, collidable2.Radius);
    }
}
