using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Commands;

public class CheckCollisionWithPredictionCommand : ICommand
{
    private readonly Vector _pos1;
    private readonly Vector _vel1;
    private readonly int _radius1;
    private readonly Vector _pos2;
    private readonly Vector _vel2;
    private readonly int _radius2;
    private readonly int _timeSteps;

    public CheckCollisionWithPredictionCommand(
        Vector pos1, Vector vel1, int radius1,
        Vector pos2, Vector vel2, int radius2,
        int timeSteps = 5)
    {
        _pos1 = pos1;
        _vel1 = vel1;
        _radius1 = radius1;
        _pos2 = pos2;
        _vel2 = vel2;
        _radius2 = radius2;
        _timeSteps = timeSteps;
    }

    public void Execute()
    {
        for (int t = 0; t <= _timeSteps; t++)
        {
            var p1 = _pos1 + t * _vel1;
            var p2 = _pos2 + t * _vel2;

            var dx = p1[0] - p2[0];
            var dy = p1[1] - p2[1];
            var distanceSquared = dx * dx + dy * dy;
            var radiusSum = _radius1 + _radius2;

            if (distanceSquared <= radiusSum * radiusSum)
            {
                throw new CollisionException(
                    $"Collision predicted between objects at time step {t}.");
            }
        }
    }
}
