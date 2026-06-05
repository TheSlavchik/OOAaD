using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CheckCollisionsBatchCommandTests
{
    [Fact]
    public void Execute_NoCollisions_OnCollisionCommandNotExecuted()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(1);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(10, 10));
        mock2.SetupGet(m => m.Radius).Returns(1);

        var mock3 = new Mock<ICollidable>();
        mock3.SetupGet(m => m.Position).Returns(new Vector(20, 20));
        mock3.SetupGet(m => m.Radius).Returns(1);

        var mockOnCollision = new Mock<ICommand>();

        var command = new CheckCollisionsBatchCommand(
            new[] { mock1.Object, mock2.Object, mock3.Object },
            mockOnCollision.Object);

        command.Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void Execute_HasCollision_OnCollisionCommandExecuted()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(5);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(3, 4));
        mock2.SetupGet(m => m.Radius).Returns(5);

        var mock3 = new Mock<ICollidable>();
        mock3.SetupGet(m => m.Position).Returns(new Vector(20, 20));
        mock3.SetupGet(m => m.Radius).Returns(1);

        var mockOnCollision = new Mock<ICommand>();

        var command = new CheckCollisionsBatchCommand(
            new[] { mock1.Object, mock2.Object, mock3.Object },
            mockOnCollision.Object);

        command.Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.AtLeastOnce);
    }

    [Fact]
    public void Execute_WithSpecificIndices_OnlyChecksSpecifiedObjects()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(5);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(3, 4));
        mock2.SetupGet(m => m.Radius).Returns(5);

        var mock3 = new Mock<ICollidable>();
        mock3.SetupGet(m => m.Position).Returns(new Vector(20, 20));
        mock3.SetupGet(m => m.Radius).Returns(1);

        var mockOnCollision = new Mock<ICommand>();

        var command = new CheckCollisionsBatchCommand(
            new[] { mock1.Object, mock2.Object, mock3.Object },
            mockOnCollision.Object,
            collidingIndices: new[] { 0, 1 });

        command.Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_WithSingleIndex_DoesNothing()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(5);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(10, 10));
        mock2.SetupGet(m => m.Radius).Returns(1);

        var mockOnCollision = new Mock<ICommand>();

        var command = new CheckCollisionsBatchCommand(
            new[] { mock1.Object, mock2.Object },
            mockOnCollision.Object,
            collidingIndices: new[] { 0 });

        command.Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void Execute_MultipleCollisions_OnCollisionCommandCalledOnce()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(10);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock2.SetupGet(m => m.Radius).Returns(10);

        var mock3 = new Mock<ICollidable>();
        mock3.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock3.SetupGet(m => m.Radius).Returns(10);

        var mockOnCollision = new Mock<ICommand>();

        var command = new CheckCollisionsBatchCommand(
            new[] { mock1.Object, mock2.Object, mock3.Object },
            mockOnCollision.Object);

        command.Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Exactly(3));
    }

    [Fact]
    public void Execute_EmptyArray_DoesNotThrow()
    {
        var mockOnCollision = new Mock<ICommand>();

        var command = new CheckCollisionsBatchCommand(
            Array.Empty<ICollidable>(),
            mockOnCollision.Object);

        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
    }
}
