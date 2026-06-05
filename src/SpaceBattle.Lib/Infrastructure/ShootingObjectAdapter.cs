using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Infrastructure;

public class ShootingObjectAdapter : IShootingObject
{
    private readonly IDictionary<string, object> _data;

    public ShootingObjectAdapter(IDictionary<string, object> data)
    {
        _data = data;
    }

    public Vector Position
    {
        get
        {
            if (!_data.TryGetValue("position", out var val) || val is not Vector position)
                throw new InvalidOperationException("Position is not available or has invalid format");
            return position;
        }
    }

    public Angle Angle
    {
        get
        {
            if (!_data.TryGetValue("angle", out var val) || val is not Angle angle)
                throw new InvalidOperationException("Angle is not available or has invalid format");
            return angle;
        }
    }
}
