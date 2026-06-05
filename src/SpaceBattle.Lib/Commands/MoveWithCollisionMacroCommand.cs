using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Commands;

public class MoveWithCollisionMacroCommand : ICommand
{
    private readonly ICollidable _torpedo;
    private readonly ICollidable _target;
    private readonly IMovable _movable;
    private readonly Guid _torpedoId;
    private readonly IGameObjectRepository _repository;
    private readonly ICommand _damageCommand;

    public MoveWithCollisionMacroCommand(
        ICollidable torpedo,
        ICollidable target,
        IMovable movable,
        Guid torpedoId,
        IGameObjectRepository repository,
        ICommand damageCommand)
    {
        _torpedo = torpedo;
        _target = target;
        _movable = movable;
        _torpedoId = torpedoId;
        _repository = repository;
        _damageCommand = damageCommand;
    }

    public void Execute()
    {
        var checkCollision = new CheckCollisionCommand(_torpedo, _target);

        try
        {
            checkCollision.Execute();
        }
        catch (CollisionException)
        {
            var destroyTorpedo = new DestroyCommand(_torpedoId, _repository);
            destroyTorpedo.Execute();
            _damageCommand.Execute();
            return;
        }

        var moveCommand = new MoveCommand(_movable);
        moveCommand.Execute();
    }
}
