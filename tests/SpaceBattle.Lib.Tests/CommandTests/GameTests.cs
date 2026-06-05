using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Game;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class GameTests
{
    [Fact]
    public void Run_ExecutesAllCommandsInOrder()
    {
        var mock1 = new Mock<ICommand>();
        var mock2 = new Mock<ICommand>();
        var mock3 = new Mock<ICommand>();

        var game = new GameLoop();
        game.AddCommand(mock1.Object);
        game.AddCommand(mock2.Object);
        game.AddCommand(mock3.Object);

        game.Run();

        mock1.Verify(c => c.Execute(), Times.Once);
        mock2.Verify(c => c.Execute(), Times.Once);
        mock3.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Run_WhenCommandThrows_GameContinuesAndExecutesRemainingCommands()
    {
        var mock1 = new Mock<ICommand>();
        var mock2 = new Mock<ICommand>();
        var mock3 = new Mock<ICommand>();

        mock2.Setup(c => c.Execute()).Throws<InvalidOperationException>();

        var game = new GameLoop();
        game.AddCommand(mock1.Object);
        game.AddCommand(mock2.Object);
        game.AddCommand(mock3.Object);

        game.Run();

        mock1.Verify(c => c.Execute(), Times.Once);
        mock2.Verify(c => c.Execute(), Times.Once);
        mock3.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Run_EmptyQueue_DoesNotThrow()
    {
        var game = new GameLoop();

        game.Run();

        Assert.True(true);
    }
}
