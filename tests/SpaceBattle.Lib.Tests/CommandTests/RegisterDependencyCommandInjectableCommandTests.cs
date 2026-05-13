using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterDependencyCommandInjectableCommandTests
{
    public RegisterDependencyCommandInjectableCommandTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Commands.CommandInjectable"));
    }

    [Fact]
    public void Execute_RegistersDependency_AndCommandInjectableResolvesAsICommandICommandInjectableAndCommandInjectableCommand()
    {
        IoC.Clear();
        var registerCommand = new RegisterDependencyCommandInjectableCommand();
        registerCommand.Execute();

        var command1 = IoC.Resolve<ICommand>("Commands.CommandInjectable");
        var command2 = IoC.Resolve<ICommandInjectable>("Commands.CommandInjectable");
        var command3 = IoC.Resolve<CommandInjectableCommand>("Commands.CommandInjectable");

        Assert.IsType<CommandInjectableCommand>(command1);
        Assert.IsType<CommandInjectableCommand>(command2);
        Assert.IsType<CommandInjectableCommand>(command3);
    }
}
