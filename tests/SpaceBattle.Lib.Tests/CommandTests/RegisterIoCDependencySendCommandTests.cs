using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencySendCommandTests
{
    public RegisterIoCDependencySendCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        var commandMock = new Mock<ICommand>();
        var receiverMock = new Mock<ICommandReceiver>();

        Assert.Throws<InvalidOperationException>(() =>
            IoC.Resolve<ICommand>("Commands.Send", commandMock.Object, receiverMock.Object));
    }

    [Fact]
    public void Execute_RegistersDependency_AndSendCommandResolves()
    {
        var commandMock = new Mock<ICommand>();
        var receiverMock = new Mock<ICommandReceiver>();

        var registerCommand = new RegisterIoCDependencySendCommand();
        registerCommand.Execute();

        var sendCommand = IoC.Resolve<ICommand>("Commands.Send", commandMock.Object, receiverMock.Object);

        Assert.IsType<SendCommand>(sendCommand);
    }
}
