using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyRotateCommand : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.Rotate", args =>
        {
            var obj = (IDictionary<string, object>)args[0];
            var rotatable = IoC.Resolve<IRotatable>("Adapters.IRotatingObject", obj);
            return new RotateCommand(rotatable);
        });
    }
}
