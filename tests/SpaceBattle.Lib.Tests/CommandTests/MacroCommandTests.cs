using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class MacroCommandTests
{
    [Fact]
    public void Execute_AllCommandsInArrayAreExecuted()
    {
        var mock1 = new Mock<ICommand>();
        var mock2 = new Mock<ICommand>();
        var mock3 = new Mock<ICommand>();

        var commands = new ICommand[] { mock1.Object, mock2.Object, mock3.Object };
        var macroCommand = new MacroCommand(commands);

        macroCommand.Execute();

        mock1.Verify(c => c.Execute(), Times.Once);
        mock2.Verify(c => c.Execute(), Times.Once);
        mock3.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_WhenCommandThrowsException_RemainingCommandsAreNotExecuted()
    {
        var mock1 = new Mock<ICommand>();
        var mock2 = new Mock<ICommand>();
        var mock3 = new Mock<ICommand>();

        mock2.Setup(c => c.Execute()).Throws<InvalidOperationException>();

        var commands = new ICommand[] { mock1.Object, mock2.Object, mock3.Object };
        var macroCommand = new MacroCommand(commands);

        Assert.Throws<InvalidOperationException>(() => macroCommand.Execute());

        mock1.Verify(c => c.Execute(), Times.Once);
        mock3.Verify(c => c.Execute(), Times.Never);
    }
}
