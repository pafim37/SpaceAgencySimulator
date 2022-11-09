using Sas.Domain.Orbits;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Domain.Tests
{
    public class FactoryOrbitTest
    {
        [Fact]
        public void Test1()
        {
            var orbit = OrbitFactory.Factory(new Vector(0.5, 0.25, 0.75), Vector.Ones, 1);
        }
    }
}
