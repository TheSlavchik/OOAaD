using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class CheckAuthorizationCommand : ICommand
{
    private readonly IDictionary<string, object> _gameObject;
    private readonly string _playerToken;

    public CheckAuthorizationCommand(IDictionary<string, object> gameObject, string playerToken)
    {
        _gameObject = gameObject;
        _playerToken = playerToken;
    }

    public void Execute()
    {
        if (!_gameObject.TryGetValue("owner", out var ownerToken) ||
            ownerToken is not string ownerString ||
            ownerString != _playerToken)
        {
            throw new UnauthorizedAccessException("Player is not authorized to control this object");
        }
    }
}
