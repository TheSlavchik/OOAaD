using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class ShootCommandTests
{
    private readonly Mock<IGameObjectRepository> _mockRepo = new();
    private readonly Mock<ICommandReceiver> _mockReceiver = new();
    private const int TorpedoSpeed = 3;
    private const int Offset = 2;

    public ShootCommandTests()
    {
        Angle.Denominator = 8;
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenAuthorized_CreatesTorpedoAndSendsMoveCommand()
    {
        var shipObject = new Dictionary<string, object>
        {
            { "owner", "player-1" },
            { "position", new Vector(0, 0) },
            { "angle", new Angle(0) }
        };

        var capturedTorpedo = new Dictionary<string, object>();
        var capturedId = Guid.Empty;

        _mockRepo.Setup(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<Guid, IDictionary<string, object>>((id, obj) =>
            {
                capturedId = id;
                capturedTorpedo.Clear();
                foreach (var kvp in obj) capturedTorpedo[kvp.Key] = kvp.Value;
            });

        _mockRepo.Setup(r => r.GetById(It.IsAny<Guid>()))
            .Returns<Guid>(id => id == capturedId ? capturedTorpedo : null);

        var mockMovable = new Mock<IMovable>();

        IoC.Register("Adapters.IMovingObject", args =>
        {
            return mockMovable.Object;
        });

        var command = new ShootCommand(
            shipObject, "player-1", _mockRepo.Object, _mockReceiver.Object, TorpedoSpeed, Offset);

        command.Execute();

        _mockRepo.Verify(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        _mockReceiver.Verify(r => r.Receive(It.IsAny<ICommand>()), Times.Once);
    }

    [Fact]
    public void Execute_WhenNotAuthorized_ThrowsUnauthorizedAccessExceptionAndDoesNotAddTorpedo()
    {
        var shipObject = new Dictionary<string, object>
        {
            { "owner", "player-1" },
            { "position", new Vector(0, 0) },
            { "angle", new Angle(0) }
        };

        var command = new ShootCommand(
            shipObject, "player-99", _mockRepo.Object, _mockReceiver.Object, TorpedoSpeed, Offset);

        Assert.Throws<UnauthorizedAccessException>(() => command.Execute());

        _mockRepo.Verify(r => r.Add(It.IsAny<Guid>(), It.IsAny<IDictionary<string, object>>()), Times.Never);
        _mockReceiver.Verify(r => r.Receive(It.IsAny<ICommand>()), Times.Never);
    }
}
