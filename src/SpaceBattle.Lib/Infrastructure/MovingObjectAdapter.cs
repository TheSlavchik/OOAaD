using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Infrastructure;

public class MovingObjectAdapter : IMovable
{
    private readonly IDictionary<string, object> _data;

    public MovingObjectAdapter(IDictionary<string, object> data)
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
        set
        {
            _data["position"] = value;
        }
    }

    public Vector Velocity
    {
        get
        {
            if (!_data.TryGetValue("velocity", out var val) || val is not Vector velocity)
                throw new InvalidOperationException("Velocity is not available or has invalid format");
            return velocity;
        }
    }
}
