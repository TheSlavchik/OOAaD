using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class CheckCollisionsBatchCommand : ICommand
{
    private readonly ICollidable[] _objects;
    private readonly ICommand _onCollisionCommand;
    private readonly int[]? _collidingIndices;

    public CheckCollisionsBatchCommand(
        ICollidable[] objects,
        ICommand onCollisionCommand,
        int[]? collidingIndices = null)
    {
        _objects = objects;
        _onCollisionCommand = onCollisionCommand;
        _collidingIndices = collidingIndices;
    }

    public void Execute()
    {
        if (_collidingIndices != null && _collidingIndices.Length >= 2)
        {
            for (int i = 0; i < _collidingIndices.Length - 1; i++)
            {
                for (int j = i + 1; j < _collidingIndices.Length; j++)
                {
                    CheckPair(_collidingIndices[i], _collidingIndices[j]);
                }
            }
        }
        else
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                for (int j = i + 1; j < _objects.Length; j++)
                {
                    CheckPair(i, j);
                }
            }
        }
    }

    private void CheckPair(int i, int j)
    {
        var checkCollision = new CheckCollisionCommand(_objects[i], _objects[j]);

        try
        {
            checkCollision.Execute();
        }
        catch (CollisionException)
        {
            _onCollisionCommand.Execute();
        }
    }
}
