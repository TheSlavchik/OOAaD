using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Abstractions;

public interface IShootingObject
{
    public Vector Position { get; }
    public Angle Angle { get; }
}
