using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CommandInjectableCommandTests
{
    [Fact]
    public void Execute_CallsInjectedCommand()
    {
        var innerMock = new Mock<ICommand>();
        var injectableCommand = new CommandInjectableCommand();

        injectableCommand.Inject(innerMock.Object);
        injectableCommand.Execute();

        innerMock.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_ThrowsExceptionWhenNoCommandInjected()
    {
        var injectableCommand = new CommandInjectableCommand();

        Assert.Throws<InvalidOperationException>(() => injectableCommand.Execute());
    }
}
