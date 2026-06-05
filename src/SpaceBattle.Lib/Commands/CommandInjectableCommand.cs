using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class CommandInjectableCommand : ICommand, ICommandInjectable
{
    private ICommand? _command;

    public void Inject(ICommand command)
    {
        _command = command;
    }

    public void Execute()
    {
        if (_command is null)
        {
            throw new InvalidOperationException("No command has been injected.");
        }

        _command.Execute();
    }
}
