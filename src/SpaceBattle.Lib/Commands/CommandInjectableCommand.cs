using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class CommandInjectableCommand : ICommand, ICommandInjectable
{
    private ICommand? _innerCommand;

    public void Inject(ICommand command)
    {
        _innerCommand = command;
    }

    public void Execute()
    {
        if (_innerCommand is null)
        {
            throw new InvalidOperationException("No command has been injected.");
        }

        _innerCommand.Execute();
    }
}
