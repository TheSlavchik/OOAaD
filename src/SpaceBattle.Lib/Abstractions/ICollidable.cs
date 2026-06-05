using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Abstractions;

public interface ICollidable
{
    public Vector Position { get; }
    public int Radius { get; }
}
