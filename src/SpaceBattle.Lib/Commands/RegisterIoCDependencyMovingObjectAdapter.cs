using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyMovingObjectAdapter : ICommand
{
    public void Execute()
    {
        IoC.Register("Adapters.IMovingObject", args =>
        {
            var obj = (IDictionary<string, object>)args[0];
            return new MovingObjectAdapter(obj);
        });
    }
}
