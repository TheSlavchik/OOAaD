using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class DestroyCommandTests
{
    [Fact]
    public void Execute_RemovesObjectFromRepository()
    {
        var objectId = Guid.NewGuid();
        var mockRepo = new Mock<IGameObjectRepository>();

        var command = new DestroyCommand(objectId, mockRepo.Object);

        command.Execute();

        mockRepo.Verify(r => r.Remove(objectId), Times.Once);
    }

    [Fact]
    public void Execute_WhenObjectDoesNotExist_DoesNotThrow()
    {
        var objectId = Guid.NewGuid();
        var mockRepo = new Mock<IGameObjectRepository>();

        mockRepo.Setup(r => r.Remove(objectId))
            .Callback(() => { });

        var command = new DestroyCommand(objectId, mockRepo.Object);

        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
        mockRepo.Verify(r => r.Remove(objectId), Times.Once);
    }
}
