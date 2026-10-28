using FluentAssertions;
using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Tests;

public class NumericalRegressionTest
{
    [Fact]
    public void MutationInvalidatesMagnitudeAndUnitState()
    {
        Vector vector = new(3, 4, 0);
        vector.Magnitude.Should().Be(5);
        vector[0] = 0;
        vector.Magnitude.Should().Be(4);
        vector.GetNormalize().Magnitude.Should().BeApproximately(1, 1e-15);
        Vector axis = Vector.Oz;
        axis.IsNormalize.Should().BeTrue();
        axis[2] = 2;
        axis.IsNormalize.Should().BeFalse();
        Vector rotated = Rotation.Rotate(Vector.Ox, axis, Math.PI / 2);
        Close(rotated[0], 0);
        Close(rotated[1], 1);
        Close(rotated.Magnitude, 1);
    }

    [Fact]
    public void VectorOwnsInputAndReturnedArrays()
    {
        double[] elements = [3, 4, 0];
        Vector vector = new(elements);
        elements[0] = 100;
        vector.GetElements()[1] = 100;
        vector.GetElements().Should().Equal(3, 4, 0);
        vector.Magnitude.Should().Be(5);
        Vector unit = Vector.Ox;
        unit.GetNormalize()[0] = 9;
        unit.X.Should().Be(1);
    }

    [Theory]
    [InlineData(1e-300)]
    [InlineData(1e300)]
    public void MagnitudeAndNormalizationHandleExtremeScales(double scale)
    {
        Vector vector = new(3 * scale, 4 * scale, 0);
        (vector.Magnitude / scale).Should().BeApproximately(5, 1e-14);
        Vector unit = vector.GetNormalize();
        Close(unit[0], 0.6);
        Close(unit[1], 0.8);
        Close(unit.Magnitude, 1);
    }

    [Fact]
    public void NormalizationSupportsFiniteComponentsWhoseNormOverflows()
    {
        Vector vector = new(double.MaxValue, double.MaxValue, 0);
        vector.GetNormalize().Magnitude.Should().BeApproximately(1, 1e-15);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void CannotNormalizeUndefinedDirections(double component)
    {
        Assert.Throws<InvalidOperationException>(() => new Vector(component, 0, 0).GetNormalize());
    }

    [Fact]
    public void EmptyVectorHasZeroNormButNoDirectionOrColumnMatrix()
    {
        Vector empty = new([]);
        empty.Magnitude.Should().Be(0);
        empty.IsNormalize.Should().BeFalse();
        Assert.Throws<InvalidOperationException>(() => empty.GetNormalize());
        Assert.Throws<InvalidOperationException>(() => empty.AsMatrix());
    }

    [Fact]
    public void AllDimensionsParticipateInEqualityAndHashing()
    {
        Vector first = new([1, 2, 3, 4]);
        Vector equal = new([1, 2, 3, 4]);
        Vector different = new([1, 2, 3, -4]);
        first.Equals(different).Should().BeFalse();
        (first == equal).Should().BeTrue();
        first.GetHashCode().Should().Be(equal.GetHashCode());
        new HashSet<Vector> { first }.Contains(equal).Should().BeTrue();
        Vector? missing = null;
        (missing == null).Should().BeTrue();
        (missing == first).Should().BeFalse();
        (first != missing).Should().BeTrue();
    }

    [Fact]
    public void SmallComponentsAreNotRoundedOutOfCalculations()
    {
        Vector vector = new(1e-14, 0, 0);
        vector.X.Should().Be(vector[0]);
        Vector.DotProduct(vector, vector).Should().BeApproximately(1e-28, 1e-42);
        vector.AsMatrix()[1, 1].Should().Be(1e-14);
    }

    [Fact]
    public void DotProductAndColumnConversionUseAllDimensions()
    {
        Vector vector = new([1, 2, 3, 4]);
        Vector.DotProduct(vector, vector).Should().Be(30);
        Vector.DotProduct(vector, new Vector(new double[] { 2 })).Should().Be(2);
        vector.AsMatrix().RowsNumber.Should().Be(4);
        vector.AsMatrix().ColumnsNumber.Should().Be(1);
        vector.AsMatrix().ToVector().Should().Be(vector);
        vector.ToString().Should().Be("<1, 2, 3, 4>");
        Assert.Throws<ArgumentException>(() => Vector.CrossProduct(vector, Vector.Ox));
        Assert.Throws<ArgumentException>(() => Vector.CrossProduct(Vector.Ox, vector));
    }

    [Fact]
    public void UnequalLengthSubtractionIsAntisymmetric()
    {
        Vector a = new([2]);
        Vector b = new([1, 3]);
        (a - b).GetElements().Should().Equal(1, -3);
        (a - b).Should().Be(-(b - a));
    }

    [Theory]
    [InlineData(2, 3, 1, new double[] { 1, 4 })]
    [InlineData(2, 3, 3, new double[] { 3, 6 })]
    [InlineData(3, 2, 1, new double[] { 1, 3, 5 })]
    [InlineData(3, 2, 2, new double[] { 2, 4, 6 })]
    public void RectangularColumnsUseColumnStride(int rows, int columns, int column, double[] expected)
    {
        new Matrix([1, 2, 3, 4, 5, 6], rows, columns).GetColumn(column).Should().Equal(expected);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(4)]
    public void ColumnIndicesAreOneBasedAndValidated(int column)
    {
        Assert.Throws<IndexOutOfRangeException>(() => new Matrix([1, 2, 3, 4, 5, 6], 2, 3).GetColumn(column));
    }

    [Theory]
    [InlineData(2, 0.5)]
    [InlineData(-2, -0.5)]
    public void OneByOneMatrixCanBeInverted(double value, double inverse)
    {
        Matrix matrix = new([value], 1, 1);
        matrix.GetDeterminant().Should().Be(value);
        matrix.Invert().Should().BeSameAs(matrix);
        matrix[0].Should().Be(inverse);
    }

    [Fact]
    public void SingularInverseLeavesOriginalUnchanged()
    {
        Matrix matrix = new([1, 2, 2, 4], 2, 2);
        matrix.GetDeterminant().Should().Be(0);
        Assert.Throws<InvalidOperationException>(() => matrix.Invert());
        matrix.GetAllElements().Should().Equal(1, 2, 2, 4);
        Assert.Throws<InvalidOperationException>(() => new Matrix([0], 1, 1).Invert());
        Assert.Throws<InvalidOperationException>(() => new Matrix([1, 2], 1, 2).Invert());
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void LuRejectsNonfiniteInput(double value)
    {
        Assert.Throws<InvalidOperationException>(() => new Matrix([value], 1, 1).GetDeterminant());
        Assert.Throws<InvalidOperationException>(() => new Matrix([value], 1, 1).Invert());
    }

    [Fact]
    public void PivotingChangesDeterminantSignAndHandlesZeroLeadingDiagonal()
    {
        Matrix matrix = new([0, 2, 1, 3], 2, 2);
        matrix.GetDeterminant().Should().BeApproximately(-2, 1e-14);
        double[] expected = [-1.5, 1, 0.5, 0];
        Matrix inverse = new Matrix(matrix.GetAllElements(), 2, 2).Invert();
        for (int i = 0; i < expected.Length; i++) Close(inverse[i], expected[i]);
        Identity(matrix * inverse);
        matrix.GetAllElements().Should().Equal(0, 2, 1, 3);
    }

    [Fact]
    public void OverflowInLuDoesNotSilentlyProduceAnIncorrectInverse()
    {
        double max = double.MaxValue;
        Matrix matrix = new([max, max, -max, max], 2, 2);
        Assert.Throws<InvalidOperationException>(() => matrix.Invert());
        matrix.GetAllElements().Should().Equal(max, max, -max, max);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(8)]
    [InlineData(20)]
    public void InverseSatisfiesBothIdentityProducts(int size)
    {
        Random random = new(17);
        double[] elements = new double[size * size];
        for (int row = 0; row < size; row++)
            for (int col = 0; col < size; col++)
                elements[row * size + col] = row == col ? size + 1 : random.NextDouble() - 0.5;
        Matrix matrix = new(elements, size, size);
        Matrix inverse = new Matrix(elements, size, size).Invert();
        Identity(matrix * inverse);
        Identity(inverse * matrix);
        Matrix identity = new Matrix(new double[size * size], size, size);
        for (int i = 1; i <= size; i++) identity[i, i] = 1;
        (matrix * identity).Should().Be(matrix);
        Matrix twiceTransposed = new Matrix(elements, size, size).Transpose().Transpose();
        twiceTransposed.Should().Be(matrix);
    }

    [Theory]
    [InlineData(1e-200)]
    [InlineData(1e200)]
    public void InverseDoesNotDependOnRepresentableDeterminant(double scale)
    {
        Matrix matrix = new([scale, 0, 0, scale], 2, 2);
        Matrix inverse = new Matrix(matrix.GetAllElements(), 2, 2).Invert();
        Identity(matrix * inverse);
    }

    [Fact]
    public void MatrixOwnsArraysAndConversions()
    {
        double[] elements = [1, 2, 3, 4];
        Matrix matrix = new(elements, 2, 2);
        elements[0] = 100;
        matrix.GetAllElements()[1] = 100;
        matrix.GetAllElements().Should().Equal(1, 2, 3, 4);
        Vector vector = matrix.ToVector();
        matrix[0] = 20;
        vector[0].Should().Be(1);
        vector[1] = 30;
        matrix[1].Should().Be(2);
        Matrix column = vector.AsMatrix();
        column[0] = -1;
        vector[0].Should().Be(1);
    }

    [Fact]
    public void EqualMatricesHaveEqualHashesAcrossDifferentArrays()
    {
        Matrix left = new([1, 2, 3, 4], 2, 2);
        Matrix right = new([1, 2, 3, 4], 2, 2);
        left.Should().Be(right);
        left.GetHashCode().Should().Be(right.GetHashCode());
        new HashSet<Matrix> { left }.Contains(right).Should().BeTrue();
    }

    [Theory]
    [InlineData(2, 3, "1, 2, 3, \n4, 5, 6, \n")]
    [InlineData(3, 2, "1, 2, \n3, 4, \n5, 6, \n")]
    [InlineData(1, 6, "1, 2, 3, 4, 5, 6, \n")]
    public void MatrixTextUsesEveryElementInRowOrder(int rows, int columns, string expected)
    {
        new Matrix([1, 2, 3, 4, 5, 6], rows, columns).ToString().Should().Be(expected);
        new Matrix([1, 2, 3], 3, 1).ToString().Should().Be("1, \n2, \n3, \n");
        new Matrix([1, 2, 3, 4], 2, 2).ToString().Should().Be("1, 2, \n3, 4, \n");
    }

    [Fact]
    public void MatrixVectorProductSupportsRectangularMatrices()
    {
        Matrix matrix = new([1, 2, 3, 4, 5, 6], 2, 3);
        (matrix * new Vector(2, -1, 3)).GetElements().Should().Equal(9, 21);
        Assert.Throws<ArgumentException>(() => matrix * new Vector(new double[] { 1, 2 }));
        Assert.Throws<InvalidOperationException>(() => matrix * matrix);
    }

    [Theory]
    [InlineData(0.3)]
    [InlineData(-1.7)]
    [InlineData(3.141592653589793)]
    public void ArbitraryAxisRotationPreservesNormAndIsReversible(double angle)
    {
        Vector input = new(2, -4, 7);
        Vector axis = new(2, 3, -1);
        Vector rotated = Rotation.Rotate(input, axis, angle);
        Close(rotated.Magnitude, input.Magnitude);
        Vector restored = Rotation.Rotate(rotated, axis, angle, inverseRotation: true);
        Vector scaledAxisResult = Rotation.Rotate(input, axis * 9, angle);
        for (int i = 0; i < 3; i++)
        {
            Close(restored[i], input[i]);
            Close(scaledAxisResult[i], rotated[i]);
        }
        input.GetElements().Should().Equal(2, -4, 7);
        axis.GetElements().Should().Equal(2, 3, -1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void InvalidRotationAxisIsRejected(double coordinate)
    {
        Assert.Throws<ArgumentException>(() => Rotation.Rotate(Vector.Ox, new Vector(coordinate, 0, 0), 1));
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.NegativeInfinity)]
    public void NonfiniteAnglesAreRejected(double angle)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Rotation.Rotate(Vector.Ox, Vector.Oz, angle));
        Assert.Throws<ArgumentOutOfRangeException>(() => Rotation.RotationMatrix3D(angle, 0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Rotation.RotationMatrix3D(0, angle, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Rotation.RotationMatrix3D(0, 0, angle));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0.37, -0.82, 1.23)]
    [InlineData(-2.1, 1.5, -0.9)]
    public void OrbitalRotationHasCorrectCompositionAndIsOrthogonal(double periapsis, double inclination, double node)
    {
        Matrix rotation = Rotation.RotationMatrix3D(periapsis, inclination, node);
        Vector input = new(2, 5, -3);
        Vector expected = Rotation.Rotate(Rotation.Rotate(Rotation.Rotate(input, Vector.Oz, periapsis), Vector.Ox, inclination), Vector.Oz, node);
        Vector actual = rotation * input;
        for (int i = 0; i < 3; i++) Close(actual[i], expected[i]);
        Matrix transpose = new Matrix(rotation.GetAllElements(), 3, 3).Transpose();
        Identity(rotation * transpose);
        Identity(transpose * rotation);
        Close(rotation.GetDeterminant(), 1);
    }

    private static void Identity(Matrix matrix)
    {
        for (int row = 1; row <= matrix.RowsNumber; row++)
            for (int col = 1; col <= matrix.ColumnsNumber; col++)
                Close(matrix[row, col], row == col ? 1 : 0);
    }

    private static void Close(double actual, double expected) =>
        actual.Should().BeApproximately(expected, 1e-12 * Math.Max(1, Math.Abs(expected)));
}
