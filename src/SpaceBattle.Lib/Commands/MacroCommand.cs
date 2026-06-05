using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class MacroCommand : ICommand
{
    private readonly ICommand[] _commands;

    public MacroCommand(ICommand[] commands)
    {
        _commands = commands;
    }

    public void Execute()
    {
        ExecuteCommands(0);
    }

    private void ExecuteCommands(int index)
    {
        if (index >= _commands.Length) return;

        _commands[index].Execute();
        ExecuteCommands(index + 1);
    }
}
