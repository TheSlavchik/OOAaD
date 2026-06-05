using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class MoveWithCollisionMacroCommandTests
{
    [Fact]
    public void Execute_WhenNoCollision_MovesTorpedoAndDoesNotDestroy()
    {
        var torpedoId = Guid.NewGuid();

        var mockTorpedo = new Mock<ICollidable>();
        mockTorpedo.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mockTorpedo.SetupGet(m => m.Radius).Returns(1);

        var mockTarget = new Mock<ICollidable>();
        mockTarget.SetupGet(m => m.Position).Returns(new Vector(10, 10));
        mockTarget.SetupGet(m => m.Radius).Returns(1);

        var mockMovable = new Mock<IMovable>();
        mockMovable.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mockMovable.SetupGet(m => m.Velocity).Returns(new Vector(2, 2));

        var mockRepo = new Mock<IGameObjectRepository>();

        var mockDamageCommand = new Mock<ICommand>();

        var command = new MoveWithCollisionMacroCommand(
            mockTorpedo.Object,
            mockTarget.Object,
            mockMovable.Object,
            torpedoId,
            mockRepo.Object,
            mockDamageCommand.Object);

        command.Execute();

        mockRepo.Verify(r => r.Remove(torpedoId), Times.Never);
        mockDamageCommand.Verify(c => c.Execute(), Times.Never);
        mockMovable.VerifySet(m => m.Position = new Vector(2, 2), Times.Once);
    }

    [Fact]
    public void Execute_WhenCollision_DestroysTorpedoAndAppliesDamageAndDoesNotMove()
    {
        var torpedoId = Guid.NewGuid();

        var mockTorpedo = new Mock<ICollidable>();
        mockTorpedo.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mockTorpedo.SetupGet(m => m.Radius).Returns(5);

        var mockTarget = new Mock<ICollidable>();
        mockTarget.SetupGet(m => m.Position).Returns(new Vector(3, 4));
        mockTarget.SetupGet(m => m.Radius).Returns(5);

        var mockMovable = new Mock<IMovable>();

        var mockRepo = new Mock<IGameObjectRepository>();

        var mockDamageCommand = new Mock<ICommand>();

        var command = new MoveWithCollisionMacroCommand(
            mockTorpedo.Object,
            mockTarget.Object,
            mockMovable.Object,
            torpedoId,
            mockRepo.Object,
            mockDamageCommand.Object);

        command.Execute();

        mockRepo.Verify(r => r.Remove(torpedoId), Times.Once);
        mockDamageCommand.Verify(c => c.Execute(), Times.Once);
        mockMovable.VerifySet(m => m.Position = It.IsAny<Vector>(), Times.Never);
    }

    [Fact]
    public void Execute_WhenCollisionOnEdge_DestroysTorpedoAndAppliesDamageAndDoesNotMove()
    {
        var torpedoId = Guid.NewGuid();

        var mockTorpedo = new Mock<ICollidable>();
        mockTorpedo.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mockTorpedo.SetupGet(m => m.Radius).Returns(3);

        var mockTarget = new Mock<ICollidable>();
        mockTarget.SetupGet(m => m.Position).Returns(new Vector(4, 0));
        mockTarget.SetupGet(m => m.Radius).Returns(1);

        var mockMovable = new Mock<IMovable>();

        var mockRepo = new Mock<IGameObjectRepository>();

        var mockDamageCommand = new Mock<ICommand>();

        var command = new MoveWithCollisionMacroCommand(
            mockTorpedo.Object,
            mockTarget.Object,
            mockMovable.Object,
            torpedoId,
            mockRepo.Object,
            mockDamageCommand.Object);

        command.Execute();

        mockRepo.Verify(r => r.Remove(torpedoId), Times.Once);
        mockDamageCommand.Verify(c => c.Execute(), Times.Once);
        mockMovable.VerifySet(m => m.Position = It.IsAny<Vector>(), Times.Never);
    }
}
