using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CreateMacroCommandStrategyTests
{
    public CreateMacroCommandStrategyTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Resolve_WhenSpecAndAllCommandsRegistered_MacroCommandExecutesAllCommands()
    {
        var mockCommand1 = new Mock<ICommand>();
        var mockCommand2 = new Mock<ICommand>();

        IoC.Register("Specs.Test", _ => new[] { "Command1", "Command2" });
        IoC.Register("Commands.Command1", _ => mockCommand1.Object);
        IoC.Register("Commands.Command2", _ => mockCommand2.Object);

        var strategy = new CreateMacroCommandStrategy("Test");
        var macroCommand = strategy.Resolve(Array.Empty<object>());

        Assert.IsType<MacroCommand>(macroCommand);

        macroCommand.Execute();

        mockCommand1.Verify(c => c.Execute(), Times.Once);
        mockCommand2.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Resolve_WhenSpecNotRegistered_ThrowsInvalidOperationException()
    {
        var strategy = new CreateMacroCommandStrategy("NonExistent");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(Array.Empty<object>()));
    }

    [Fact]
    public void Resolve_WhenCommandNotRegistered_ThrowsInvalidOperationException()
    {
        IoC.Register("Specs.Test", _ => new[] { "Command1", "Command2" });
        IoC.Register("Commands.Command1", _ => new Mock<ICommand>().Object);

        var strategy = new CreateMacroCommandStrategy("Test");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(Array.Empty<object>()));
    }
}
