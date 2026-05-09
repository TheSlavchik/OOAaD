using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.DataTests;

public class VectorTests
{
    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        var v = new Vector(10, 20, 30);

        Assert.Equal(10, v[0]);
        Assert.Equal(20, v[1]);
        Assert.Equal(30, v[2]);
    }

    [Fact]
    public void Indexer_Set_UpdatesValue()
    {
        var v = new Vector(1, 2, 3);
        v[1] = 99;

        Assert.Equal(99, v[1]);
        Assert.Equal(new Vector(1, 99, 3), v);
    }

    [Fact]
    public void Size_ReturnsCorrectLength()
    {
        var v = new Vector(1, 2, 3, 4);
    
        Assert.Equal(4, v.Size);
    }
    
    [Fact]
    public void Create_VectorWithNoCoordinates_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Vector());
    }

    [Fact]
    public void Add_Vectors_ReturnsCorrectResult()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(4, 5, 6);
        var result = v1 + v2;

        Assert.Equal(new Vector(5, 7, 9), result);
    }
    
    [Fact]
    public void Add_VectorsWithOppositeCoordinates_ReturnsZeroVector()
    {
        var v1 = new Vector(1, -1, 2);
        var v2 = new Vector(-1, 1, -2);
        var result = v1 + v2;

        Assert.Equal(new Vector(0, 0, 0), result);
    }

    [Fact]
    public void Add_VectorsWithDifferentDimensions_LeftLonger_ThrowsArgumentException()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2);

        Assert.Throws<ArgumentException>(() => v1 + v2);
    }

    [Fact]
    public void Add_VectorsWithDifferentDimensions_RightLonger_ThrowsArgumentException()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(1, 2, 3);

        Assert.Throws<ArgumentException>(() => v1 + v2);
    }
    
    [Fact]
    public void Subtract_Vectors_ReturnsCorrectResult()
    {
        var v1 = new Vector(5, 3, 2);
        var v2 = new Vector(1, 2, 1);
        var result = v1 - v2;

        Assert.Equal(new Vector(4, 1, 1), result);
    }

    [Fact]
    public void Subtract_VectorsWithDifferentDimensions_LeftLonger_ThrowsArgumentException()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2);

        Assert.Throws<ArgumentException>(() => v1 - v2);
    }

    [Fact]
    public void Subtract_VectorsWithDifferentDimensions_RightLonger_ThrowsArgumentException()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(1, 2, 3);

        Assert.Throws<ArgumentException>(() => v1 - v2);
    }
    
    [Fact]
    public void Multiply_ScalarWithVector_ReturnsCorrectResult()
    {
        var v = new Vector(1, 2, 3);
        var result = 2 * v;

        Assert.Equal(new Vector(2, 4, 6), result);
    }

    [Fact]
    public void Multiply_ZeroWithVector_ReturnsZeroVector()
    {
        var v = new Vector(1, 2, 3);
        var result = 0 * v;

        Assert.Equal(new Vector(0, 0, 0), result);
    }

    [Fact]
    public void Multiply_NegativeScalarWithVector_ReturnsCorrectResult()
    {
        var v = new Vector(1, -2, 3);
        var result = -1 * v;

        Assert.Equal(new Vector(-1, 2, -3), result);
    }

    [Fact]
    public void Equals_CoordinatesMatch_DifferentObjects_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);

        Assert.True(v1.Equals(v2));
    }
    
    [Fact]
    public void Equals_CompareWithNull_ReturnsFalse()
    {
        var v = new Vector(1, 2, 3);

        Assert.False(v.Equals(null));
    }

    [Fact]
    public void Equals_CompareWithDifferentType_ReturnsFalse()
    {
        var v = new Vector(1, 2, 3);

        Assert.False(v.Equals("not a vector"));
    }

    [Fact]
    public void OperatorEquals_CoordinatesMatch_DifferentObjects_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);

        Assert.True(v1 == v2);
    }

    [Fact]
    public void Equals_CoordinatesDiffer_ReturnsFalse()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 4);

        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void Equals_SameVector_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);

        Assert.True(v1.Equals(v1));
    }

    [Fact]
    public void OperatorEquals_SameVector_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);

        Assert.True(v1 == v1);
    }

    [Fact]
    public void OperatorEqual_CompareNull_ReturnsTrue()
    {
        Vector? v1 = null;
        Vector? v2 = null;

        Assert.True(v1 == v2);
    }

    [Fact]
    public void OperatorEqual_CompareNullAndVector_ReturnsFalse()
    {
        Vector v1 = new Vector(1, 2, 3);
        Vector? v2 = null;

        Assert.False(v1 == v2);
    }

    [Fact]
    public void OperatorNotEqual_CoordinatesDiffer_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 4);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void GetHashCode_VectorHasHashCode()
    {
        var v = new Vector(1, 2, 3);
        var hashCode = v.GetHashCode();

        Assert.NotEqual(0, hashCode);
    }
    
    [Fact]
    public void GetHashCode_SameVectors_HaveSameHashCode()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);

        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentVectors_HaveDifferentHashCode()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(4, 5, 6);

        Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
    }
}
