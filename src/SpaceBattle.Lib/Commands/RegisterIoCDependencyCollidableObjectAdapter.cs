using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyCollidableObjectAdapter : ICommand
{
    public void Execute()
    {
        IoC.Register("Adapters.ICollidableObject", args =>
        {
            var obj = (IDictionary<string, object>)args[0];
            return new CollidableObjectAdapter(obj);
        });
    }
}
