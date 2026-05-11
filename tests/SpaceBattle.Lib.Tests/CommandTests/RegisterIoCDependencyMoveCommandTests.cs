using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterIoCDependencyMoveCommandTests
{
    public RegisterIoCDependencyMoveCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.Move", new Dictionary<string, object>()));
    }

    [Fact]
    public void Execute_RegistersDependency_AndMoveCommandResolves()
    {
        var obj = new Dictionary<string, object>();

        var mockMovable = new Mock<IMovable>();
        IoC.Register("Adapters.IMovingObject", args =>
        {
            var gameObj = (IDictionary<string, object>)args[0];
            Assert.Same(obj, gameObj);
            return mockMovable.Object;
        });

        var registerCommand = new RegisterIoCDependencyMoveCommand();
        registerCommand.Execute();

        var moveCommand = IoC.Resolve<ICommand>("Commands.Move", obj);

        Assert.IsType<MoveCommand>(moveCommand);
    }

    [Fact]
    public void Execute_WhenAdapterNotRegistered_ThrowsInvalidOperationException()
    {
        var obj = new Dictionary<string, object>();

        var registerCommand = new RegisterIoCDependencyMoveCommand();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.Move", obj));
    }
}
