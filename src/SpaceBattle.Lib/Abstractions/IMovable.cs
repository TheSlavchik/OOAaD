using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Abstractions;

public interface IMovable
{
    public Vector Position { get; set; }
    public Vector Velocity { get; }
}
