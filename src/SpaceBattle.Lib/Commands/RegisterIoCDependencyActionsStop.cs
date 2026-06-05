using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyActionsStop : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.CommandInjectable", args => new CommandInjectableCommand());

        IoC.Register("Actions.Stop", args =>
        {
            var order = (IDictionary<string, object>)args[0];
            return IoC.Resolve<ICommand>("Commands.CommandInjectable");
        });
    }
}
