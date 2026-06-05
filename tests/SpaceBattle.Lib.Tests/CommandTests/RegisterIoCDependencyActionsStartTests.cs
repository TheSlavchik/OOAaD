using Moq;
using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Tests.CommandTests;

[Collection("IoC")]
public class RegisterIoCDependencyActionsStartTests
{
    public RegisterIoCDependencyActionsStartTests()
    {
        IoC.Clear();
    }

    [Fact]
    public void Execute_WhenResolvingUnregisteredDependency_ThrowsInvalidOperationException()
    {
        var order = new Dictionary<string, object>();
        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Actions.Start", order));
    }

    [Fact]
    public void Execute_RegistersDependency_ActionsStartResolves()
    {
        var order = new Dictionary<string, object>();

        var registerCommand = new RegisterIoCDependencyActionsStart();
        registerCommand.Execute();

        var command = IoC.Resolve<ICommand>("Actions.Start", order);

        Assert.IsType<CommandInjectableCommand>(command);
    }
}
