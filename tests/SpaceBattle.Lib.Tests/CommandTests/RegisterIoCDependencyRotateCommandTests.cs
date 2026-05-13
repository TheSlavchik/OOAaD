using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterIoCDependencyRotateCommandTests
{
    public RegisterIoCDependencyRotateCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.Rotate", new Dictionary<string, object>()));
    }

    [Fact]
    public void Execute_RegistersDependency_AndRotateCommandResolves()
    {
        IoC.Clear();
        var obj = new Dictionary<string, object>();

        var mockRotatable = new Mock<IRotatable>();
        IoC.Register("Adapters.IRotatingObject", args =>
        {
            var gameObj = (IDictionary<string, object>)args[0];
            Assert.Same(obj, gameObj);
            return mockRotatable.Object;
        });

        var registerCommand = new RegisterIoCDependencyRotateCommand();
        registerCommand.Execute();

        var rotateCommand = IoC.Resolve<ICommand>("Commands.Rotate", obj);

        Assert.IsType<RotateCommand>(rotateCommand);
    }

    [Fact]
    public void Execute_WhenAdapterNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        var obj = new Dictionary<string, object>();

        var registerCommand = new RegisterIoCDependencyRotateCommand();
        registerCommand.Execute();

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.Rotate", obj));
    }
}
