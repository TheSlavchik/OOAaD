using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterIoCDependencySendCommandTests
{
    public RegisterIoCDependencySendCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        var mockCommand = new Mock<ICommand>();
        var mockReceiver = new Mock<ICommandReceiver>();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.Send", mockCommand.Object, mockReceiver.Object));
    }

    [Fact]
    public void Execute_RegistersDependency_AndSendCommandResolves()
    {
        IoC.Clear();
        var mockCommand = new Mock<ICommand>();
        var mockReceiver = new Mock<ICommandReceiver>();

        var registerCommand = new RegisterIoCDependencySendCommand();
        registerCommand.Execute();

        var sendCommand = IoC.Resolve<ICommand>("Commands.Send", mockCommand.Object, mockReceiver.Object);

        Assert.IsType<SendCommand>(sendCommand);
    }
}
