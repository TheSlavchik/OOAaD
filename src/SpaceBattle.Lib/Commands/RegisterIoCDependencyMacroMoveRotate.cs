using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        IoC.Register("Macro.Move", args =>
        {
            var strategy = new CreateMacroCommandStrategy("Move");
            return strategy.Resolve(args);
        });

        IoC.Register("Macro.Rotate", args =>
        {
            var strategy = new CreateMacroCommandStrategy("Rotate");
            return strategy.Resolve(args);
        });
    }
}
