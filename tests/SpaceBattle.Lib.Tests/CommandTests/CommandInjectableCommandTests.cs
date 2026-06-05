using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CommandInjectableCommandTests
{
    [Fact]
    public void Execute_AfterInject_InvokesInjectedCommand()
    {
        var mockCommand = new Mock<ICommand>();
        var injectableCommand = new CommandInjectableCommand();

        injectableCommand.Inject(mockCommand.Object);
        injectableCommand.Execute();

        mockCommand.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_WithoutInject_ThrowsInvalidOperationException()
    {
        var injectableCommand = new CommandInjectableCommand();

        Assert.Throws<InvalidOperationException>(() => injectableCommand.Execute());
    }
}
