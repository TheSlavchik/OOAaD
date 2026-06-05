using SpaceBattle.Lib.Abstractions;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Infrastructure;

public class CollidableObjectAdapter : ICollidable
{
    private readonly IDictionary<string, object> _data;

    public CollidableObjectAdapter(IDictionary<string, object> data)
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

    public int Radius
    {
        get
        {
            if (!_data.TryGetValue("radius", out var val) || val is not int radius)
                throw new InvalidOperationException("Radius is not available or has invalid format");
            return radius;
        }
    }
}
