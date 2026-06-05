using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CheckCollisionCommandTests
{
    [Fact]
    public void Execute_ObjectsDoNotCollide_NoExceptionThrown()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(1);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(10, 10));
        mock2.SetupGet(m => m.Radius).Returns(1);

        var command = new CheckCollisionCommand(mock1.Object, mock2.Object);

        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
    }

    [Fact]
    public void Execute_ObjectsCollide_ThrowsCollisionException()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(5);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(3, 4));
        mock2.SetupGet(m => m.Radius).Returns(5);

        var command = new CheckCollisionCommand(mock1.Object, mock2.Object);

        Assert.Throws<CollisionException>(() => command.Execute());
    }

    [Fact]
    public void Execute_ObjectsTouch_ThrowsCollisionException()
    {
        var mock1 = new Mock<ICollidable>();
        mock1.SetupGet(m => m.Position).Returns(new Vector(0, 0));
        mock1.SetupGet(m => m.Radius).Returns(3);

        var mock2 = new Mock<ICollidable>();
        mock2.SetupGet(m => m.Position).Returns(new Vector(4, 0));
        mock2.SetupGet(m => m.Radius).Returns(1);

        var command = new CheckCollisionCommand(mock1.Object, mock2.Object);

        Assert.Throws<CollisionException>(() => command.Execute());
    }
}
