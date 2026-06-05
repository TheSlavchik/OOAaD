using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencyMovingObjectAdapterTests
{
    public RegisterIoCDependencyMovingObjectAdapterTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IoC.Resolve<IMovable>("Adapters.IMovingObject", new Dictionary<string, object>()));
    }

    [Fact]
    public void Execute_RegistersDependency_AndMovingObjectAdapterResolves()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) },
            { "velocity", new Vector(-4, 1) }
        };

        var registerCommand = new RegisterIoCDependencyMovingObjectAdapter();
        registerCommand.Execute();

        var movable = IoC.Resolve<IMovable>("Adapters.IMovingObject", obj);

        Assert.NotNull(movable);
        Assert.IsType<MovingObjectAdapter>(movable);
        Assert.Equal(obj["position"], movable.Position);
        Assert.Equal(obj["velocity"], movable.Velocity);
    }

    [Fact]
    public void Execute_ResolvedAdapter_WorksWithMoveCommand()
    {
        var obj = new Dictionary<string, object>
        {
            { "position", new Vector(12, 5) },
            { "velocity", new Vector(-4, 1) }
        };

        var registerCommand = new RegisterIoCDependencyMovingObjectAdapter();
        registerCommand.Execute();

        var movable = IoC.Resolve<IMovable>("Adapters.IMovingObject", obj);
        var moveCommand = new MoveCommand(movable);
        moveCommand.Execute();

        Assert.Equal(new Vector(8, 6), movable.Position);
    }
}
