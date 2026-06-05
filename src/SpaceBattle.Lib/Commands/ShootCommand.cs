using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;
using SpaceBattle.Lib.Infrastructure;

namespace SpaceBattle.Lib.Commands;

public class ShootCommand : ICommand
{
    private readonly IDictionary<string, object> _shipObject;
    private readonly string _playerToken;
    private readonly IGameObjectRepository _repository;
    private readonly ICommandReceiver _receiver;
    private readonly int _torpedoSpeed;
    private readonly int _offset;

    public ShootCommand(
        IDictionary<string, object> shipObject,
        string playerToken,
        IGameObjectRepository repository,
        ICommandReceiver receiver,
        int torpedoSpeed,
        int offset)
    {
        _shipObject = shipObject;
        _playerToken = playerToken;
        _repository = repository;
        _receiver = receiver;
        _torpedoSpeed = torpedoSpeed;
        _offset = offset;
    }

    public void Execute()
    {
        var authCommand = new CheckAuthorizationCommand(_shipObject, _playerToken);

        var shooter = new ShootingObjectAdapter(_shipObject);
        var createTorpedoCommand = new CreateTorpedoCommand(shooter, _repository, _torpedoSpeed, _offset);

        var macroCommand = new MacroCommand([authCommand, createTorpedoCommand]);
        macroCommand.Execute();

        var torpedo = _repository.GetById(createTorpedoCommand.CreatedTorpedoId!.Value)!;
        var movable = IoC.Resolve<IMovable>("Adapters.IMovingObject", torpedo);
        var moveCommand = new MoveCommand(movable);
        var sendCommand = new SendCommand(moveCommand, _receiver);
        sendCommand.Execute();
    }
}
