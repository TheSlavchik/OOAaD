namespace SpaceBattle.Lib.Data;

public class Angle
{
    private static int _denominator = 8;
    private readonly int _numerator;

    public static int Denominator
    {
        get => _denominator;
        set => _denominator = value;
    }

    public int Numerator => _numerator;

    public Angle(int numerator)
    {
        _numerator = ((numerator % _denominator) + _denominator) % _denominator;
    }

    private int NormalizedNumerator => ((_numerator % _denominator) + _denominator) % _denominator;

    public static Angle operator +(Angle a1, Angle a2)
    {
        return new Angle(a1._numerator + a2._numerator);
    }

    public static implicit operator double(Angle angle)
    {
        return (angle.NormalizedNumerator * 2.0 * Math.PI) / _denominator;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is Angle other)
        {
            return NormalizedNumerator == other.NormalizedNumerator;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NormalizedNumerator, _denominator);
    }

    public static bool operator ==(Angle? a1, Angle? a2)
    {
        if (ReferenceEquals(a1, a2))
            return true;
        if (a1 is null || a2 is null)
            return false;
        return a1.Equals(a2);
    }

    public static bool operator !=(Angle? a1, Angle? a2)
    {
        return !(a1 == a2);
    }
}
