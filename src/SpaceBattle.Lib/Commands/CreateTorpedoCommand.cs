using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Commands;

public class CreateTorpedoCommand : ICommand
{
    private readonly IShootingObject _shooter;
    private readonly IGameObjectRepository _repository;
    private readonly int _torpedoSpeed;
    private readonly int _offset;

    public CreateTorpedoCommand(IShootingObject shooter, IGameObjectRepository repository, int torpedoSpeed, int offset)
    {
        _shooter = shooter;
        _repository = repository;
        _torpedoSpeed = torpedoSpeed;
        _offset = offset;
    }

    public void Execute()
    {
        var shooterPosition = _shooter.Position;
        var shooterAngle = _shooter.Angle;

        var direction = AngleToDirection(shooterAngle);
        var offsetVector = new Vector(direction[0] * _offset, direction[1] * _offset);
        var torpedoPosition = shooterPosition + offsetVector;

        var velocityVector = new Vector(direction[0] * _torpedoSpeed, direction[1] * _torpedoSpeed);

        var torpedo = new Dictionary<string, object>
        {
            { "position", torpedoPosition },
            { "velocity", velocityVector },
            { "angle", new Angle(0) }
        };

        var torpedoId = Guid.NewGuid();
        _repository.Add(torpedoId, torpedo);
    }

    private static int[] AngleToDirection(Angle angle)
    {
        var radians = (angle.Numerator * 2.0 * Math.PI) / Angle.Denominator;
        var dx = (int)Math.Round(Math.Cos(radians));
        var dy = (int)Math.Round(Math.Sin(radians));
        return new[] { dx, dy };
    }
}
