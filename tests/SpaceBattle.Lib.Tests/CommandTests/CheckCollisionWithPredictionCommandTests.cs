using SpaceBattle.Lib.Commands;
using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.CommandTests;

public class CheckCollisionWithPredictionCommandTests
{
    [Fact]
    public void Execute_ObjectsMovingTowardsEachOther_CollisionPredicted()
    {
        var pos1 = new Vector(0, 0);
        var vel1 = new Vector(1, 0);
        var radius1 = 1;

        var pos2 = new Vector(10, 0);
        var vel2 = new Vector(-1, 0);
        var radius2 = 1;

        var command = new CheckCollisionWithPredictionCommand(
            pos1, vel1, radius1, pos2, vel2, radius2, timeSteps: 10);

        Assert.Throws<CollisionException>(() => command.Execute());
    }

    [Fact]
    public void Execute_ObjectsMovingAway_NoCollisionPredicted()
    {
        var pos1 = new Vector(0, 0);
        var vel1 = new Vector(1, 0);
        var radius1 = 1;

        var pos2 = new Vector(10, 0);
        var vel2 = new Vector(2, 0);
        var radius2 = 1;

        var command = new CheckCollisionWithPredictionCommand(
            pos1, vel1, radius1, pos2, vel2, radius2, timeSteps: 10);

        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
    }

    [Fact]
    public void Execute_ObjectsCurrentlyOverlapping_CollisionAtCurrentTimeStep()
    {
        var pos1 = new Vector(0, 0);
        var vel1 = new Vector(0, 0);
        var radius1 = 5;

        var pos2 = new Vector(3, 4);
        var vel2 = new Vector(0, 0);
        var radius2 = 5;

        var command = new CheckCollisionWithPredictionCommand(
            pos1, vel1, radius1, pos2, vel2, radius2, timeSteps: 5);

        Assert.Throws<CollisionException>(() => command.Execute());
    }

    [Fact]
    public void Execute_ObjectsWillCollideInFuture_CollisionPredicted()
    {
        var pos1 = new Vector(0, 0);
        var vel1 = new Vector(2, 0);
        var radius1 = 1;

        var pos2 = new Vector(20, 0);
        var vel2 = new Vector(-2, 0);
        var radius2 = 1;

        var command = new CheckCollisionWithPredictionCommand(
            pos1, vel1, radius1, pos2, vel2, radius2, timeSteps: 20);

        Assert.Throws<CollisionException>(() => command.Execute());
    }

    [Fact]
    public void Execute_DefaultTimeSteps_DoesNotThrow()
    {
        var pos1 = new Vector(0, 0);
        var vel1 = new Vector(1, 0);
        var radius1 = 1;

        var pos2 = new Vector(20, 0);
        var vel2 = new Vector(0, 0);
        var radius2 = 1;

        var command = new CheckCollisionWithPredictionCommand(
            pos1, vel1, radius1, pos2, vel2, radius2);

        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
    }
}
