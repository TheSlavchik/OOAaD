namespace SpaceBattle.Lib.Data;

public class Vector
{
    private readonly int[] _values;
    
    public int Size => _values.Length;

    public Vector(params int[] nums)
    {
        if (nums.Length == 0)
        {
            throw new ArgumentException("Vector cannot be empty.");
        }

        _values = nums.ToArray();
    }

    public int this[int index]
    {
        get => _values[index];
        set => _values[index] = value;
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size)
        {
            throw new ArgumentException("Vectors must have the same size.");
        }

        var result = v1._values.Zip(v2._values, (a, b) => a + b).ToArray();
        return new Vector(result);
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size)
        {
            throw new ArgumentException("Vectors must have the same size.");
        }

        var result = v1._values.Zip(v2._values, (a, b) => a - b).ToArray();
        return new Vector(result);
    }

    public static Vector operator *(int alpha, Vector v1)
    {
        var result = v1._values.Select(x => x * alpha).ToArray();
        return new Vector(result);
    }

    public static bool operator ==(Vector? v1, Vector? v2)
    {
        if (ReferenceEquals(v1, v2))
            return true;
        if (v1 is null || v2 is null) 
            return false;
        return v1.Equals(v2);
    }

    public static bool operator !=(Vector? v1, Vector? v2)
    {
        return !(v1 == v2);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector other && _values.SequenceEqual(other._values);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var val in _values)
        {
            hash.Add(val);
        }
        return hash.ToHashCode();
    }
}
