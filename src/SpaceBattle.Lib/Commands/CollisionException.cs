namespace SpaceBattle.Lib.Commands;

public class CollisionException : Exception
{
    public CollisionException()
    {
    }

    public CollisionException(string message) : base(message)
    {
    }

    public CollisionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
