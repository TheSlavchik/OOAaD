using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencyShootCommandTests
{
    public RegisterIoCDependencyShootCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        var shipObject = new Dictionary<string, object>();
        var receiver = new Mock<ICommandReceiver>();

        Assert.Throws<InvalidOperationException>(() =>
            IoC.Resolve<ICommand>("Commands.Shoot", shipObject, "player-1", receiver.Object, 3, 2));
    }

    [Fact]
    public void Execute_AfterRegistration_ResolvesShootCommand()
    {
        var shipObject = new Dictionary<string, object>();
        var receiver = new Mock<ICommandReceiver>();

        var registerGameRepo = new RegisterIoCDependencyGameRepository();
        registerGameRepo.Execute();

        var registerShoot = new RegisterIoCDependencyShootCommand();
        registerShoot.Execute();

        var command = IoC.Resolve<ICommand>("Commands.Shoot", shipObject, "player-1", receiver.Object, 3, 2);

        Assert.NotNull(command);
        Assert.IsType<ShootCommand>(command);
    }
}
