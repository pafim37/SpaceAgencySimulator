using NUnit.Framework;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Tests
{
    [TestFixture]
    public class VectorTest
    {
        [Test]
        public void CreateVectorWhenPassingThreeComponents()
        {
            Vector vector = new Vector(11, 13, 17);

            Assert.IsNotNull(vector);
            Assert.AreEqual(vector.X, 11);
            Assert.AreEqual(vector.Y, 13);
            Assert.AreEqual(vector.Z, 17);
        }

        [Test]
        public void CreateVectorWhenPassingTwoComponents()
        {
            Vector vector = new Vector(11, 13);

            Assert.IsNotNull(vector);
            Assert.AreEqual(vector.X, 11);
            Assert.AreEqual(vector.Y, 13);
            Assert.AreEqual(vector.Z, 0);
        }

        [TestCase(new double[] {11, 13, 17}, 11, 13, 17)]
        [TestCase(new double[] {11, 13, 17, 19}, 11, 13, 17)]
        [TestCase(new double[] {11, 13}, 11, 13, 0)]
        [TestCase(new double[] {11}, 11, 0, 0)]
        public void CreateVectorWhenPassingArray(double[] elements, double expectedX, double expectedY, double expectedZ)
        {
            Vector vector = new Vector(elements);

            Assert.IsNotNull(vector);
            Assert.AreEqual(expectedX, vector.X);
            Assert.AreEqual(expectedY, vector.Y);
            Assert.AreEqual(expectedZ, vector.Z);
        }

        [Test]
        public void CreateVectorZero()
        {
            Vector vector = Vector.Zero;

            Assert.IsNotNull(vector);
            Assert.AreEqual(0, vector.X);
            Assert.AreEqual(0, vector.Y);
            Assert.AreEqual(0, vector.Z);
        }

        [TestCase(new double[] { 2 }, 2)]
        [TestCase(new double[] { 3, 4 }, 5)]
        [TestCase(new double[] { -6, -8 }, 10)]
        public void VectorReturnsMagnitude(double[] elements, double magnitude)
        {
            Vector vector = new Vector(elements);

            Assert.IsNotNull(vector);
            Assert.AreEqual(magnitude, vector.Magnitude);
        }

        [TestCase(new double[] {0, 0, 0, 0}, 4)]
        [TestCase(new double[] {0, 0, 0}, 3)]
        [TestCase(new double[] {0, 0}, 2)]
        [TestCase(new double[] { 0 }, 1)]
        public void VectorReturnLength(double[] elements, int length)
        {
            Vector vector = new Vector(elements);

            Assert.IsNotNull(vector);
            Assert.AreEqual(length, vector.Length);

        }

        [TestCase(new double[] { 11, 13, 17, 19 }, 11, 13, 17, 19)]
        public void GetElementsAreAccesibleByBrackets(double[] elements, double expectedX, double expectedY, double expectedZ, double expectedT)
        {
            Vector vector = new Vector(elements);

            Assert.IsNotNull(vector);
            Assert.AreEqual(expectedX, vector[0]);
            Assert.AreEqual(expectedY, vector[1]);
            Assert.AreEqual(expectedZ, vector[2]);
            Assert.AreEqual(expectedT, vector[3]);
        }

        [TestCase(new double[] { 1, 3, 5, 7, 9 }, new double[] { 2, 4, 6 }, new double[] { 3, 7, 11, 7, 9 })]
        [TestCase(new double[] { 2, 4, 6 }, new double[] { 1, 3, 5, 7, 9 }, new double[] { 3, 7, 11, 7, 9 })]
        [TestCase(new double[] { 1, 1, 1, 1, 1 }, new double[] { 2, 2, 2, 2, 2 }, new double[] { 3, 3, 3, 3, 3 })]
        public void AddTwoVectorsReturnsSumVectors(double[] elements1, double[] elements2, double[] expectedElements)
        {
            Vector vector1 = new Vector(elements1);
            Vector vector2 = new Vector(elements2);
            Vector vector3 = new Vector(expectedElements);

            Vector result = vector1 + vector2;

            Assert.AreEqual(result[0], vector3[0]);      
            Assert.AreEqual(result[1], vector3[1]);      
            Assert.AreEqual(result[2], vector3[2]);      
            Assert.AreEqual(result[3], vector3[3]);      
            Assert.AreEqual(result[4], vector3[4]);      

        }

        [TestCase(new double[] { 1, 3, 5, 7, 9 }, new double[] { 2, 4, 6 }, new double[] { -1, -1, -1, 7, 9 })]
        [TestCase(new double[] { 2, 4, 6 }, new double[] { 1, 3, 5, 7, 9 }, new double[] { 1, 1, 1, 7, 9 })]
        [TestCase(new double[] { 1, 1, 1, 1, 1 }, new double[] { 2, 2, 2, 2, 2 }, new double[] { -1, -1, -1, -1, -1 })]
        public void SubtractionTwoVectorsReturnsVectors(double[] elements1, double[] elements2, double[] expected)
        {
            Vector vector1 = new Vector(elements1);
            Vector vector2 = new Vector(elements2);
            Vector vector3 = new Vector(expected);

            Vector result = vector1 - vector2;

            Assert.AreEqual(result[0], vector3[0]);
            Assert.AreEqual(result[1], vector3[1]);
            Assert.AreEqual(result[2], vector3[2]);
            Assert.AreEqual(result[3], vector3[3]);
            Assert.AreEqual(result[4], vector3[4]);
        }

        [TestCase(new double[] { 1, 3, 5, 7, 9 }, new double[] { 2, 4, 6 }, new double[] { 2, 12, 30, 7, 9 })]
        [TestCase(new double[] { 2, -4, -6 }, new double[] { 1, -3, 5, 7, 9 }, new double[] { 2, 12, -30, 7, 9 })]
        [TestCase(new double[] { 1, 1, 1, 1, 1 }, new double[] { 2, 2, 2, 2, 2 }, new double[] { 2, 2, 2, 2, 2 })]
        public void MultipliationTwoVectorsReturnsVectors(double[] elements1, double[] elements2, double[] expected)
        {
            Vector vector1 = new Vector(elements1);
            Vector vector2 = new Vector(elements2);
            Vector vector3 = new Vector(expected);

            Vector result = vector1 * vector2;

            Assert.AreEqual(result[0], vector3[0]);
            Assert.AreEqual(result[1], vector3[1]);
            Assert.AreEqual(result[2], vector3[2]);
            Assert.AreEqual(result[3], vector3[3]);
            Assert.AreEqual(result[4], vector3[4]);
        }

        [TestCase(new double[] { 1, 3, 5, 7, 9 }, 2, new double[] { 2, 6, 10, 14, 18 })]
        public void MultipliationByRightScalarReturnsVectors(double[] elements, double scalar, double[] expected)
        {
            Vector vector1 = new Vector(elements);
            Vector vector3 = new Vector(expected);

            Vector result = vector1 * scalar;

            Assert.AreEqual(result[0], vector3[0]);
            Assert.AreEqual(result[1], vector3[1]);
            Assert.AreEqual(result[2], vector3[2]);
            Assert.AreEqual(result[3], vector3[3]);
            Assert.AreEqual(result[4], vector3[4]);
        }

        [TestCase(new double[] { 1, 3, 5, 7, 9 }, 2, new double[] { 2, 6, 10, 14, 18 })]
        public void MultipliationByLeftScalarReturnsVectors(double[] elements, double scalar, double[] expected)
        {
            Vector vector1 = new Vector(elements);
            Vector vector3 = new Vector(expected);

            Vector result = scalar * vector1;

            Assert.AreEqual(result[0], vector3[0]);
            Assert.AreEqual(result[1], vector3[1]);
            Assert.AreEqual(result[2], vector3[2]);
            Assert.AreEqual(result[3], vector3[3]);
            Assert.AreEqual(result[4], vector3[4]);
        }

        [TestCase(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 }, new double[] { -3, 6, -3 })]
        public void CrossProcuctReturnsVectors(double[] elements1, double[] elements2, double[] expectedElements)
        {
            Vector vector1 = new Vector(elements1);
            Vector vector2 = new Vector(elements2);
            Vector expectedVector = new Vector(expectedElements);

            Vector result = vector1.CrossProduct(vector2);

            Assert.AreEqual(result[0], expectedVector[0]);
            Assert.AreEqual(result[1], expectedVector[1]);
            Assert.AreEqual(result[2], expectedVector[2]);
        }

        [TestCase(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 }, new double[] { -3, 6, -3 })]
        public void CrossProcuctReturnsVectors2(double[] elements1, double[] elements2, double[] expectedElements)
        {
            Vector vector1 = new Vector(elements1);
            Vector vector2 = new Vector(elements2);
            Vector expectedVector = new Vector(expectedElements);

            Vector result = Vector.CrossProduct(vector1, vector2);

            Assert.AreEqual(result[0], expectedVector[0]);
            Assert.AreEqual(result[1], expectedVector[1]);
            Assert.AreEqual(result[2], expectedVector[2]);
        }

        [TestCase(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 }, 56)]
        public void DotProcuctReturnsScalar(double[] elements1, double[] elements2, double dotProduct)
        {
            Vector vector1 = new Vector(elements1);
            Vector vector2 = new Vector(elements2);

            double result = Vector.DotProduct(vector1, vector2);

            Assert.AreEqual(result, dotProduct);
        }
    }
}
