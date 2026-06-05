using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class MoveCommandTests
{
    [Fact]
    public void Execute_MovingObject_ChangesPositionCorrectly()
    {
        var mock = new Mock<IMovable>();
        mock.SetupGet(m => m.Position).Returns(new Vector(12, 5));
        mock.SetupGet(m => m.Velocity).Returns(new Vector(-4, 1));

        var command = new MoveCommand(mock.Object);
        command.Execute();

        mock.VerifySet(m => m.Position = new Vector(8, 6), Times.Once);
    }

    [Fact]
    public void Execute_WhenPositionCannotBeDetermined_ThrowsException()
    {
        var mock = new Mock<IMovable>();
        mock.SetupGet(m => m.Velocity).Returns(new Vector(-4, 1));
        mock.SetupGet(m => m.Position).Throws<Exception>();

        var command = new MoveCommand(mock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenVelocityCannotBeDetermined_ThrowsException()
    {
        var mock = new Mock<IMovable>();
        mock.SetupGet(m => m.Position).Returns(new Vector(12, 5));
        mock.SetupGet(m => m.Velocity).Throws<Exception>();

        var command = new MoveCommand(mock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenPositionCannotBeChanged_ThrowsException()
    {
        var mock = new Mock<IMovable>();
        mock.SetupGet(m => m.Position).Returns(new Vector(12, 5));
        mock.SetupGet(m => m.Velocity).Returns(new Vector(-4, 1));
        mock.SetupSet(m => m.Position = It.IsAny<Vector>()).Throws<Exception>();

        var command = new MoveCommand(mock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }
}
