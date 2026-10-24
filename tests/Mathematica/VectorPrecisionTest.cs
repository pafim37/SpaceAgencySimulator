using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Tests;

public class VectorPrecisionTest
{
    [Fact]
    public void OrbitalPositionPreservesSmallDisplacements()
    {
        const double distance = 149597870700;
        Vector position = new(distance, Math.PI, -distance);
        Vector moved = position + new Vector(1000, 0, 0);

        Assert.Equal(distance, position.X);
        Assert.Equal(Math.PI, position.Y);
        Assert.Equal(distance + 1000, moved.X);
        Assert.Equal(new Vector(1000, 0, 0), moved - position);
        Assert.Equal(new Vector(-distance, -Math.PI, distance), -position);
    }

    [Fact]
    public void CalculationsRetainDoublePrecision()
    {
        const double component = 16777217;
        const double scalar = 1.0000000001;
        Vector vector = new(component, 0, 0);

        Assert.Equal(component * scalar, (vector * scalar).X);
        Assert.Equal(vector * scalar, scalar * vector);
        Assert.Equal(component, vector.Magnitude);
        Assert.Equal(component, Vector.DotProduct(vector, Vector.Ox));
        Assert.Equal(new Vector(0, 0, component), Vector.CrossProduct(vector, Vector.Oy));
    }

    [Fact]
    public void EqualComponentsHaveConsistentEqualityAndHashCodes()
    {
        Vector first = new(1, 2, 3);
        Vector second = new(1, 2, 3);
        Vector different = new(1, 2, 4);

        Assert.True(first == second);
        Assert.False(first != second);
        Assert.True(first.Equals(second));
        Assert.True(first.Equals((object)second));
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        HashSet<Vector> values = [first];
        Assert.False(values.Add(second));
        Assert.False(first == different);
        Assert.True(first != different);
        Assert.False(first.Equals(different));
        Assert.False(first.Equals(new object()));
    }

    [Fact]
    public void EqualityHandlesNullOnEitherSide()
    {
        Vector? missing = null;
        Vector value = new();

        Assert.True(missing == null);
        Assert.False(missing != null);
        Assert.False(value == missing);
        Assert.False(missing == value);
        Assert.True(value != missing);
        Assert.True(missing != value);
        Assert.False(value.Equals(missing));
    }
}
