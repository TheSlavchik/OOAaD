using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class SendCommandTests
{
    [Fact]
    public void SendCommandTransmitsCommandToReceiver()
    {
        var commandMock = new Mock<ICommand>();
        var receiverMock = new Mock<ICommandReceiver>();

        var sendCommand = new SendCommand(commandMock.Object, receiverMock.Object);
        sendCommand.Execute();

        receiverMock.Verify(r => r.Receive(commandMock.Object), Times.Once);
    }

    [Fact]
    public void Execute_WhenReceiverThrowsException_ThrowsException()
    {
        var commandMock = new Mock<ICommand>();
        var receiverMock = new Mock<ICommandReceiver>();
        receiverMock.Setup(r => r.Receive(It.IsAny<ICommand>())).Throws<Exception>();

        var sendCommand = new SendCommand(commandMock.Object, receiverMock.Object);

        Assert.Throws<Exception>(() => sendCommand.Execute());
    }
}
