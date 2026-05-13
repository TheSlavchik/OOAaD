using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class RegisterIoCDependencyActionsStopTests
{
    public RegisterIoCDependencyActionsStopTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        IoC.Clear();
        var order = new Dictionary<string, object>();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Actions.Stop", order));
    }

    [Fact]
    public void Execute_RegistersDependency_AndActionsStopResolvesAsCommandInjectableCommand()
    {
        IoC.Clear();
        var order = new Dictionary<string, object>();

        var registerCommand = new RegisterIoCDependencyActionsStop();
        registerCommand.Execute();

        var command = IoC.Resolve<ICommand>("Actions.Stop", order);

        Assert.IsType<CommandInjectableCommand>(command);
    }
}
