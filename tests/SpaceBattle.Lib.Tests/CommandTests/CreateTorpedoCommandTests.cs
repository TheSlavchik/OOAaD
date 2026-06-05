using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CreateTorpedoCommandTests
{
    private readonly Mock<IGameObjectRepository> _mockRepo = new();
    private readonly Mock<IShootingObject> _mockShooter = new();
    private const int TorpedoSpeed = 3;
    private const int Offset = 2;

    public CreateTorpedoCommandTests()
    {
        Angle.Denominator = 8;
    }

    [Fact]
    public void Execute_AddsNewObjectToRepository()
    {
        _mockShooter.Setup(s => s.Position).Returns(new Vector(0, 0));
        _mockShooter.Setup(s => s.Angle).Returns(new Angle(0));

        var command = new CreateTorpedoCommand(
            _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);

        command.Execute();

        _mockRepo.Verify(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public void Execute_TorpedoPosition_IsOffsetFromShooterPosition()
    {
        var shooterPosition = new Vector(5, 5);
        _mockShooter.Setup(s => s.Position).Returns(shooterPosition);
        _mockShooter.Setup(s => s.Angle).Returns(new Angle(0));

        IDictionary<string, object>? capturedTorpedo = null;
        _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<Guid, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

        var command = new CreateTorpedoCommand(
            _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);
        command.Execute();

        Assert.NotNull(capturedTorpedo);
        var expectedPosition = new Vector(7, 5);
        var actualPosition = Assert.IsType<Vector>(capturedTorpedo["position"]);
        Assert.Equal(expectedPosition, actualPosition);
    }

    [Fact]
    public void Execute_TorpedoVelocity_BasedOnAngle()
    {
        _mockShooter.Setup(s => s.Position).Returns(new Vector(0, 0));
        _mockShooter.Setup(s => s.Angle).Returns(new Angle(2));

        IDictionary<string, object>? capturedTorpedo = null;
        _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<Guid, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

        var command = new CreateTorpedoCommand(
            _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);
        command.Execute();

        Assert.NotNull(capturedTorpedo);
        var expectedVelocity = new Vector(0, 3);
        var actualVelocity = Assert.IsType<Vector>(capturedTorpedo["velocity"]);
        Assert.Equal(expectedVelocity, actualVelocity);
    }

    [Fact]
    public void Execute_TorpedoPositionAndVelocity_WithAngle45Degrees()
    {
        _mockShooter.Setup(s => s.Position).Returns(new Vector(10, 10));
        _mockShooter.Setup(s => s.Angle).Returns(new Angle(1));

        IDictionary<string, object>? capturedTorpedo = null;
        _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<Guid, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

        var command = new CreateTorpedoCommand(
            _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);
        command.Execute();

        Assert.NotNull(capturedTorpedo);

        var expectedPosition = new Vector(12, 12);
        var actualPosition = Assert.IsType<Vector>(capturedTorpedo["position"]);
        Assert.Equal(expectedPosition, actualPosition);

        var expectedVelocity = new Vector(3, 3);
        var actualVelocity = Assert.IsType<Vector>(capturedTorpedo["velocity"]);
        Assert.Equal(expectedVelocity, actualVelocity);
    }

    [Fact]
    public void Execute_TorpedoHasAngleZero()
    {
        _mockShooter.Setup(s => s.Position).Returns(new Vector(0, 0));
        _mockShooter.Setup(s => s.Angle).Returns(new Angle(0));

        IDictionary<string, object>? capturedTorpedo = null;
        _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<Guid, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

        var command = new CreateTorpedoCommand(
            _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);
        command.Execute();

        Assert.NotNull(capturedTorpedo);
        var actualAngle = Assert.IsType<Angle>(capturedTorpedo["angle"]);
        Assert.Equal(new Angle(0), actualAngle);
    }

    [Fact]
    public void Execute_EachCall_GeneratesUniqueId()
    {
        _mockShooter.Setup(s => s.Position).Returns(new Vector(0, 0));
        _mockShooter.Setup(s => s.Angle).Returns(new Angle(0));

        var capturedIds = new List<Guid>();
        _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<Guid, IDictionary<string, object>>((id, _) => capturedIds.Add(id));

        var command = new CreateTorpedoCommand(
            _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);

        command.Execute();
        command.Execute();

        Assert.Equal(2, capturedIds.Count);
        Assert.NotEqual(capturedIds[0], capturedIds[1]);
    }

    [Fact]
    public void Execute_WithDenominator16_CorrectDirection()
    {
        Angle.Denominator = 16;

        try
        {
            _mockShooter.Setup(s => s.Position).Returns(new Vector(0, 0));
            _mockShooter.Setup(s => s.Angle).Returns(new Angle(0));

            IDictionary<string, object>? capturedTorpedo = null;
            _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
                .Callback<Guid, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

            var command = new CreateTorpedoCommand(
                _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);
            command.Execute();

            Assert.NotNull(capturedTorpedo);

            var expectedPosition = new Vector(2, 0);
            var actualPosition = Assert.IsType<Vector>(capturedTorpedo["position"]);
            Assert.Equal(expectedPosition, actualPosition);

            var expectedVelocity = new Vector(3, 0);
            var actualVelocity = Assert.IsType<Vector>(capturedTorpedo["velocity"]);
            Assert.Equal(expectedVelocity, actualVelocity);
        }
        finally
        {
            Angle.Denominator = 8;
        }
    }

    [Fact]
    public void Execute_WithDenominator16_Angle45Deg()
    {
        Angle.Denominator = 16;

        try
        {
            _mockShooter.Setup(s => s.Position).Returns(new Vector(0, 0));
            _mockShooter.Setup(s => s.Angle).Returns(new Angle(2));

            IDictionary<string, object>? capturedTorpedo = null;
            _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
                .Callback<Guid, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

            var command = new CreateTorpedoCommand(
                _mockShooter.Object, _mockRepo.Object, TorpedoSpeed, Offset);
            command.Execute();

            Assert.NotNull(capturedTorpedo);

            var expectedPosition = new Vector(2, 2);
            var actualPosition = Assert.IsType<Vector>(capturedTorpedo["position"]);
            Assert.Equal(expectedPosition, actualPosition);

            var expectedVelocity = new Vector(3, 3);
            var actualVelocity = Assert.IsType<Vector>(capturedTorpedo["velocity"]);
            Assert.Equal(expectedVelocity, actualVelocity);
        }
        finally
        {
            Angle.Denominator = 8;
        }
    }
}
