using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Abstractions;

public interface IRotatable
{
    public Angle Angle { get; set; }
    public Angle AngularVelocity { get; }
}
