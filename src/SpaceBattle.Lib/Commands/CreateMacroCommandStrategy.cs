using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class CreateMacroCommandStrategy
{
    private readonly string _commandSpec;

    public CreateMacroCommandStrategy(string commandSpec)
    {
        _commandSpec = commandSpec;
    }

    public ICommand Resolve(object[] args)
    {
        var commandNames = IoC.Resolve<string[]>($"Specs.{_commandSpec}");

        var commands = BuildCommands(commandNames, 0, new ICommand[commandNames.Length]);

        return new MacroCommand(commands);
    }

    private static ICommand[] BuildCommands(string[] commandNames, int index, ICommand[] commands)
    {
        if (index >= commandNames.Length)
        {
            return commands;
        }

        commands[index] = IoC.Resolve<ICommand>($"Commands.{commandNames[index]}");

        return BuildCommands(commandNames, index + 1, commands);
    }
}
