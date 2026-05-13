using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterDependencyCommandInjectableCommand : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.CommandInjectable", args => new CommandInjectableCommand());
    }
}
