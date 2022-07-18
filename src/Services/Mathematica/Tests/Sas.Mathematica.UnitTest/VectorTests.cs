using NUnit.Framework;
using System.Threading.Tasks;
using Xunit;

namespace Sas.Mathematica.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Theory]
        [InlineData(0, 0, 0)]
        public Task VectorMagnitudeReturnsMagnitude(double x, double y, double z)
        {

        }
    }
}