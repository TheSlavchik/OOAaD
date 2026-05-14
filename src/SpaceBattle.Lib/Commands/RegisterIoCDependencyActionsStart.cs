using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyActionsStart : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.CommandInjectable", args => new CommandInjectableCommand());

        IoC.Register("Actions.Start", args =>
        {
            var order = (IDictionary<string, object>)args[0];
            return IoC.Resolve<ICommand>("Commands.CommandInjectable");
        });
    }
}
