using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class ObjectBoundCommandTests
{
    [Fact]
    public void ObjectIdProperty_ReturnsCorrectId()
    {
        var objectId = Guid.NewGuid();
        var obj = new Dictionary<string, object>();
        var mockRepo = new Mock<IGameObjectRepository>();
        mockRepo.Setup(r => r.GetById(objectId)).Returns(obj);

        var mockInner = new Mock<ICommand>();

        var command = new ObjectBoundCommand(mockInner.Object, objectId, mockRepo.Object);

        Assert.Equal(objectId, command.ObjectId);
    }

    [Fact]
    public void Execute_WhenObjectExists_ExecutesInnerCommand()
    {
        var objectId = Guid.NewGuid();
        var obj = new Dictionary<string, object>();
        var mockRepo = new Mock<IGameObjectRepository>();
        mockRepo.Setup(r => r.GetById(objectId)).Returns(obj);

        var mockInner = new Mock<ICommand>();

        var command = new ObjectBoundCommand(mockInner.Object, objectId, mockRepo.Object);
        command.Execute();

        mockInner.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_WhenObjectDoesNotExist_SkipsInnerCommand()
    {
        var objectId = Guid.NewGuid();
        var mockRepo = new Mock<IGameObjectRepository>();
        mockRepo.Setup(r => r.GetById(objectId)).Returns((IDictionary<string, object>?)null);

        var mockInner = new Mock<ICommand>();

        var command = new ObjectBoundCommand(mockInner.Object, objectId, mockRepo.Object);
        command.Execute();

        mockInner.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void Execute_WhenObjectIsRemovedAfterCommandQueued_DoesNotThrow()
    {
        var objectId = Guid.NewGuid();
        var torpedoData = new Dictionary<string, object>
        {
            { "position", new SpaceBattle.Lib.Data.Vector(0, 0) },
            { "velocity", new SpaceBattle.Lib.Data.Vector(3, 0) }
        };

        var repository = new SpaceBattle.Lib.Infrastructure.GameObjectRepository();
        repository.Add(objectId, torpedoData);

        var movable = new SpaceBattle.Lib.Infrastructure.MovingObjectAdapter(torpedoData);
        var moveCommand = new MoveCommand(movable);
        var boundCommand = new ObjectBoundCommand(moveCommand, objectId, repository);

        repository.Remove(objectId);

        var exception = Record.Exception(() => boundCommand.Execute());
        Assert.Null(exception);
    }
}
