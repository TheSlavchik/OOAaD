using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterIoCDependencyMacroCommandTests
{
    public RegisterIoCDependencyMacroCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        var commands = Array.Empty<ICommand>();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.Macro", (object)commands));
    }

    [Fact]
    public void Execute_RegistersDependency_AndMacroCommandResolves()
    {
        IoC.Clear();
        var mockCommand1 = new Mock<ICommand>();
        var mockCommand2 = new Mock<ICommand>();
        var commands = new ICommand[] { mockCommand1.Object, mockCommand2.Object };

        var registerCommand = new RegisterIoCDependencyMacroCommand();
        registerCommand.Execute();

        var macroCommand = IoC.Resolve<ICommand>("Commands.Macro", (object)commands);

        Assert.IsType<MacroCommand>(macroCommand);
    }
}
