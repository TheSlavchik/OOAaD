using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyMoveCommand : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.Move", args =>
        {
            var obj = (IDictionary<string, object>)args[0];
            var movable = IoC.Resolve<IMovable>("Adapters.IMovingObject", obj);
            return new MoveCommand(movable);
        });
    }
}
