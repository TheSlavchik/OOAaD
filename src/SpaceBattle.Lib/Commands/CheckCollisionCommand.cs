using SpaceBattle.Lib.Abstractions;

namespace SpaceBattle.Lib.Commands;

public class CheckCollisionCommand : ICommand
{
    private readonly ICollidable _obj1;
    private readonly ICollidable _obj2;

    public CheckCollisionCommand(ICollidable obj1, ICollidable obj2)
    {
        _obj1 = obj1;
        _obj2 = obj2;
    }

    public void Execute()
    {
        var dx = _obj1.Position[0] - _obj2.Position[0];
        var dy = _obj1.Position[1] - _obj2.Position[1];
        var distanceSquared = dx * dx + dy * dy;
        var radiusSum = _obj1.Radius + _obj2.Radius;

        if (distanceSquared <= radiusSum * radiusSum)
        {
            throw new CollisionException("Collision detected between two objects.");
        }
    }
}
