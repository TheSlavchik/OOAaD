using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class ObjectBoundCommand : ICommand
{
    private readonly ICommand _innerCommand;
    private readonly Guid _objectId;
    private readonly IGameObjectRepository _repository;

    public Guid ObjectId => _objectId;

    public ObjectBoundCommand(ICommand innerCommand, Guid objectId, IGameObjectRepository repository)
    {
        _innerCommand = innerCommand;
        _objectId = objectId;
        _repository = repository;
    }

    public void Execute()
    {
        var obj = _repository.GetById(_objectId);
        if (obj == null)
        {
            return;
        }
        _innerCommand.Execute();
    }
}
