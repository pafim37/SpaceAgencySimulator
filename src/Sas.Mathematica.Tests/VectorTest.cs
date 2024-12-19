using FluentAssertions;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Tests
{
    public class VectorTest
    {
        [Fact]
        public void CreateVectorWhenPassingThreeComponents()
        {
            Vector vector = new(11, 13, 17);
            vector.X.Should().Be(11);
            vector.Y.Should().Be(13);
            vector.Z.Should().Be(17);
        }

        [Fact]
        public void CreateVectorWhenPassingOneComponent()
        {
            Vector vector = new(37);
            vector.X.Should().Be(37);
            vector.Y.Should().Be(0);
            vector.Z.Should().Be(0);
        }

        [Theory]
        [InlineData(new double[] { 11, 13, 17 }, 11, 13, 17)]
        [InlineData(new double[] { 11, 13, 17, 19 }, 11, 13, 17)]
        [InlineData(new double[] { 11, 13 }, 11, 13, 0)]
        [InlineData(new double[] { 11 }, 11, 0, 0)]
        public void CreateVectorWhenPassingArray(double[] elements, double expectedX, double expectedY, double expectedZ)
        {
            Vector vector = new(elements);
            vector.X.Should().Be(expectedX);
            vector.Y.Should().Be(expectedY);
            vector.Z.Should().Be(expectedZ);
        }

        [Fact]
        public void CreateVectorZero()
        {
            Vector vector = Vector.Zero;
            vector.X.Should().Be(0);
            vector.Y.Should().Be(0);
            vector.Z.Should().Be(0);
        }

        [Fact]
        public void CreateVectorOne()
        {
            Vector vector = Vector.Ones;
            vector.X.Should().Be(1);
            vector.Y.Should().Be(1);
            vector.Z.Should().Be(1);
        }

        [Theory]
        [InlineData(new double[] { 2 }, 2)]
        [InlineData(new double[] { 3, 4 }, 5)]
        [InlineData(new double[] { -6, -8 }, 10)]
        public void VectorReturnsMagnitude(double[] elements, double magnitude)
        {
            Vector vector = new(elements);
            vector.Magnitude.Should().Be(magnitude);
        }

        [Theory]
        [InlineData(new double[] { 0, 0, 0, 0 }, 4)]
        [InlineData(new double[] { 0, 0, 0 }, 3)]
        [InlineData(new double[] { 0, 0 }, 2)]
        [InlineData(new double[] { 0 }, 1)]
        public void VectorReturnLength(double[] elements, int length)
        {
            Vector vector = new(elements);
            vector.Length.Should().Be(length);
        }

        [Theory]
        [InlineData(new double[] { 11, 13, 17, 19 }, 11, 13, 17, 19)]
        public void GetElementsAreAccesibleByBrackets(double[] elements, double expectedX, double expectedY, double expectedZ, double expectedT)
        {
            Vector vector = new(elements);
            vector[0].Should().Be(expectedX);
            vector[1].Should().Be(expectedY);
            vector[2].Should().Be(expectedZ);
            vector[3].Should().Be(expectedT);
        }

        [Theory]
        [InlineData(new double[] { 1, 3, 5, 7, 9 }, new double[] { 2, 4, 6 }, new double[] { 3, 7, 11, 7, 9 })]
        [InlineData(new double[] { 2, 4, 6 }, new double[] { 1, 3, 5, 7, 9 }, new double[] { 3, 7, 11, 7, 9 })]
        [InlineData(new double[] { 1, 1, 1, 1, 1 }, new double[] { 2, 2, 2, 2, 2 }, new double[] { 3, 3, 3, 3, 3 })]
        public void AddTwoVectorsReturnsSumVectors(double[] elements1, double[] elements2, double[] expectedElements)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);
            Vector vector3 = new(expectedElements);

            Vector result = vector1 + vector2;

            result.Should().Be(vector3);
        }

        [Theory]
        [InlineData(new double[] { 1, 3, 5, 7, 9 }, new double[] { 2, 4, 6 }, new double[] { -1, -1, -1, 7, 9 })]
        [InlineData(new double[] { 2, 4, 6 }, new double[] { 1, 3, 5, 7, 9 }, new double[] { 1, 1, 1, 7, 9 })]
        [InlineData(new double[] { 1, 1, 1, 1, 1 }, new double[] { 2, 2, 2, 2, 2 }, new double[] { -1, -1, -1, -1, -1 })]
        public void SubtractionTwoVectorsReturnsVectors(double[] elements1, double[] elements2, double[] expected)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);
            Vector vector3 = new(expected);

            Vector result = vector1 - vector2;

            result.Should().Be(vector3);
        }

        [Theory]
        [InlineData(new double[] { 1, 3, 5, 7, 9 }, new double[] { 2, 4, 6 }, new double[] { 2, 12, 30, 7, 9 })]
        [InlineData(new double[] { 2, -4, -6 }, new double[] { 1, -3, 5, 7, 9 }, new double[] { 2, 12, -30, 7, 9 })]
        [InlineData(new double[] { 1, 1, 1, 1, 1 }, new double[] { 2, 2, 2, 2, 2 }, new double[] { 2, 2, 2, 2, 2 })]
        public void MultipliationTwoVectorsReturnsVectors(double[] elements1, double[] elements2, double[] expected)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);
            Vector vector3 = new(expected);

            Vector result = vector1 * vector2;
            result.Should().Be(vector3);
        }

        [Theory]
        [InlineData(new double[] { 1, 3, 5, 7, 9 }, 2, new double[] { 2, 6, 10, 14, 18 })]
        public void MultipliationByRightScalarReturnsVectors(double[] elements, double scalar, double[] expected)
        {
            Vector vector1 = new(elements);
            Vector vector3 = new(expected);

            Vector result = vector1 * scalar;
            result.Should().Be(vector3);
        }

        [Theory]
        [InlineData(new double[] { 1, 3, 5, 7, 9 }, 2, new double[] { 2, 6, 10, 14, 18 })]
        public void MultipliationByLeftScalarReturnsVectors(double[] elements, double scalar, double[] expected)
        {
            Vector vector1 = new(elements);
            Vector vector3 = new(expected);

            Vector result = scalar * vector1;
            result.Should().Be(vector3);
        }

        [Theory]
        [InlineData(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 }, new double[] { -3, 6, -3 })]
        public void CrossProcuctReturnsVectors(double[] elements1, double[] elements2, double[] expectedElements)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);
            Vector expectedVector = new(expectedElements);

            Vector result = vector1.CrossProduct(vector2);
            result.Should().Be(expectedVector);
        }

        [Theory]
        [InlineData(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 }, new double[] { -3, 6, -3 })]
        public void CrossProcuctReturnsVectors2(double[] elements1, double[] elements2, double[] expectedElements)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);
            Vector expectedVector = new(expectedElements);

            Vector result = Vector.CrossProduct(vector1, vector2);
            result.Should().Be(expectedVector);
        }

        [Theory]
        [InlineData(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 })]
        public void CrossProcuctAreTheSame(double[] elements1, double[] elements2)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);

            Vector result1 = Vector.CrossProduct(vector1, vector2);
            Vector result2 = vector1.CrossProduct(vector2);

            result1.Should().Be(result2);
        }

        [Theory]
        [InlineData(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 }, 56)]
        public void DotProductReturnsScalar(double[] elements1, double[] elements2, double dotProduct)
        {
            Vector vector1 = new(elements1);
            Vector vector2 = new(elements2);

            double result = Vector.DotProduct(vector1, vector2);
            result.Should().Be(dotProduct);
        }
    }

}
