using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyGameRepository : ICommand
{
    public void Execute()
    {
        IGameObjectRepository? instance = null;

        IoC.Register("Game.Repository", args =>
        {
            instance ??= new GameObjectRepository();
            return instance;
        });
    }
}
