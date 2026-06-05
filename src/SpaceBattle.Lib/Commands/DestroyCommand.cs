using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class DestroyCommand : ICommand
{
    private readonly Guid _id;
    private readonly IGameObjectRepository _repository;

    public DestroyCommand(Guid id, IGameObjectRepository repository)
    {
        _id = id;
        _repository = repository;
    }

    public void Execute()
    {
        _repository.Remove(_id);
    }
}
