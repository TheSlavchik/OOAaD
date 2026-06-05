using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CheckAuthorizationCommandTests
{
    [Fact]
    public void Execute_WhenOwnerTokenMatches_DoesNotThrow()
    {
        var gameObject = new Dictionary<string, object>
        {
            { "owner", "player-42" }
        };

        var command = new CheckAuthorizationCommand(gameObject, "player-42");

        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
    }

    [Fact]
    public void Execute_WhenOwnerTokenDoesNotMatch_ThrowsUnauthorizedAccessException()
    {
        var gameObject = new Dictionary<string, object>
        {
            { "owner", "player-42" }
        };

        var command = new CheckAuthorizationCommand(gameObject, "player-99");

        Assert.Throws<UnauthorizedAccessException>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenOwnerKeyMissing_ThrowsUnauthorizedAccessException()
    {
        var gameObject = new Dictionary<string, object>();

        var command = new CheckAuthorizationCommand(gameObject, "player-42");

        Assert.Throws<UnauthorizedAccessException>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenOwnerIsNotString_ThrowsUnauthorizedAccessException()
    {
        var gameObject = new Dictionary<string, object>
        {
            { "owner", 12345 }
        };

        var command = new CheckAuthorizationCommand(gameObject, "player-42");

        Assert.Throws<UnauthorizedAccessException>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenUsedInMacroCommand_MacroCommandThrowsUnauthorizedAccessException()
    {
        var gameObject = new Dictionary<string, object>
        {
            { "owner", "player-1" }
        };

        var authCommand = new CheckAuthorizationCommand(gameObject, "player-99");
        var mockCommand = new Mock<ICommand>();
        var macroCommand = new MacroCommand([authCommand, mockCommand.Object]);

        Assert.Throws<UnauthorizedAccessException>(() => macroCommand.Execute());
        mockCommand.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void Execute_WhenAuthorizedInMacroCommand_AllCommandsExecute()
    {
        var gameObject = new Dictionary<string, object>
        {
            { "owner", "player-1" }
        };

        var authCommand = new CheckAuthorizationCommand(gameObject, "player-1");
        var mockCommand = new Mock<ICommand>();
        var macroCommand = new MacroCommand([authCommand, mockCommand.Object]);

        macroCommand.Execute();

        mockCommand.Verify(c => c.Execute(), Times.Once);
    }
}
