using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RotateCommandTests
{
    [Fact]
    public void Execute_RotatingObject_ChangesAngleCorrectly()
    {
        var mock = new Mock<IRotatable>();
        mock.SetupGet(m => m.Angle).Returns(new Angle(1));
        mock.SetupGet(m => m.AngularVelocity).Returns(new Angle(1));

        var command = new RotateCommand(mock.Object);
        command.Execute();

        mock.VerifySet(m => m.Angle = new Angle(2), Times.Once);
    }

    [Fact]
    public void Execute_WhenAngleCannotBeDetermined_ThrowsException()
    {
        var mock = new Mock<IRotatable>();
        mock.SetupGet(m => m.AngularVelocity).Returns(new Angle(1));
        mock.SetupGet(m => m.Angle).Throws<Exception>();

        var command = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenAngularVelocityCannotBeDetermined_ThrowsException()
    {
        var mock = new Mock<IRotatable>();
        mock.SetupGet(m => m.Angle).Returns(new Angle(1));
        mock.SetupGet(m => m.AngularVelocity).Throws<Exception>();

        var command = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenAngleCannotBeChanged_ThrowsException()
    {
        var mock = new Mock<IRotatable>();
        mock.SetupGet(m => m.Angle).Returns(new Angle(1));
        mock.SetupGet(m => m.AngularVelocity).Returns(new Angle(1));
        mock.SetupSet(m => m.Angle = It.IsAny<Angle>()).Throws<Exception>();

        var command = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }
}
