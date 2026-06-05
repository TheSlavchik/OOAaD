using SpaceBattle.Lib.Data;

namespace SpaceBattle.Lib.Tests.DataTests;

public class AngleTests
{
    public AngleTests()
    {
        Angle.Denominator = 8;
    }

    [Fact]
    public void Add_Angles_ReturnsCorrectResult()
    {
        var a1 = new Angle(5);
        var a2 = new Angle(7);
        var result = a1 + a2;

        Assert.Equal(new Angle(4), result);
    }

    [Fact]
    public void Equals_NormalizedAnglesMatch_ReturnsTrue()
    {
        var a1 = new Angle(15);
        var a2 = new Angle(23);

        Assert.True(a1.Equals(a2));
    }

    [Fact]
    public void OperatorEquals_NormalizedAnglesMatch_ReturnsTrue()
    {
        var a1 = new Angle(15);
        var a2 = new Angle(23);

        Assert.True(a1 == a2);
    }

    [Fact]
    public void Equals_AnglesDiffer_ReturnsFalse()
    {
        var a1 = new Angle(1);
        var a2 = new Angle(2);

        Assert.False(a1.Equals(a2));
    }

    [Fact]
    public void OperatorNotEqual_AnglesDiffer_ReturnsTrue()
    {
        var a1 = new Angle(1);
        var a2 = new Angle(2);

        Assert.True(a1 != a2);
    }

    [Fact]
    public void GetHashCode_AngleHasHashCode()
    {
        var a = new Angle(5);
        var hashCode = a.GetHashCode();

        Assert.NotEqual(0, hashCode);
    }

    [Fact]
    public void Denominator_GetterSetter_WorksCorrectly()
    {
        var previousDenominator = Angle.Denominator;
        Angle.Denominator = 16;
        Assert.Equal(16, Angle.Denominator);
        Angle.Denominator = previousDenominator;
    }

    [Fact]
    public void Numerator_ReturnsConstructorValue()
    {
        var a = new Angle(5);
        Assert.Equal(5, a.Numerator);
    }

    [Fact]
    public void Equals_CompareWithNull_ReturnsFalse()
    {
        var a = new Angle(5);
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void Equals_CompareWithDifferentType_ReturnsFalse()
    {
        var a = new Angle(5);
        Assert.False(a.Equals("not an angle"));
    }

    [Fact]
    public void Equals_SameAngle_ReturnsTrue()
    {
        var a = new Angle(5);
        Assert.True(a.Equals(a));
    }

    [Fact]
    public void OperatorEquals_SameAngle_ReturnsTrue()
    {
        var a = new Angle(5);
        Assert.True(a == a);
    }

    [Fact]
    public void OperatorEqual_CompareNull_ReturnsTrue()
    {
        Angle? a1 = null;
        Angle? a2 = null;
        Assert.True(a1 == a2);
    }

    [Fact]
    public void OperatorEqual_CompareNullAndAngle_ReturnsFalse()
    {
        Angle a1 = new Angle(5);
        Angle? a2 = null;
        Assert.False(a1 == a2);
    }

    [Fact]
    public void OperatorEqual_CompareAngleAndNull_ReturnsFalse()
    {
        Angle? a1 = null;
        Angle a2 = new Angle(5);
        Assert.False(a1 == a2);
    }

    [Fact]
    public void OperatorNotEqual_CompareNullAndAngle_ReturnsTrue()
    {
        Angle a1 = new Angle(5);
        Angle? a2 = null;
        Assert.True(a1 != a2);
    }

    [Fact]
    public void OperatorNotEqual_CompareAngleAndNull_ReturnsTrue()
    {
        Angle? a1 = null;
        Angle a2 = new Angle(5);
        Assert.True(a1 != a2);
    }

    [Fact]
    public void Cos_FromAngle_ReturnsCorrectValue()
    {
        var angle = new Angle(2);
        var cos = Math.Cos(angle);

        var expected = Math.Cos((4 * Math.PI) / Angle.Denominator);
        Assert.Equal(expected, cos, 10);
    }

    [Fact]
    public void Sin_FromAngle_ReturnsCorrectValue()
    {
        var angle = new Angle(2);
        var sin = Math.Sin(angle);

        var expected = Math.Sin((4 * Math.PI) / Angle.Denominator);
        Assert.Equal(expected, sin, 10);
    }
}
