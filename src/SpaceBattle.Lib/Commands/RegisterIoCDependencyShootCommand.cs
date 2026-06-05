using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyShootCommand : ICommand
{
    public void Execute()
    {
        IoC.Register("Commands.Shoot", args =>
        {
            var shipObject = (IDictionary<string, object>)args[0];
            var playerToken = (string)args[1];

            var repository = IoC.Resolve<IGameObjectRepository>("Game.Repository");

            var receiver = (ICommandReceiver)args[2];

            var torpedoSpeed = (int)args[3];
            var offset = (int)args[4];

            return new ShootCommand(shipObject, playerToken, repository, receiver, torpedoSpeed, offset);
        });
    }
}
