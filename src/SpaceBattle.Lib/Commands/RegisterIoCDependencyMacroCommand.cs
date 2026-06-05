using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyMacroCommand : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.Macro", args =>
        {
            var commands = (ICommand[])args[0];
            return new MacroCommand(commands);
        });
    }
}
