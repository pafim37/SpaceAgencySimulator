using FluentAssertions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test.Points
{
    public class GetHyperbolicOrbitPointsTest
    {
        [Fact]
        public void GetHyperbolicOrbitPointsReturnPoints()
        {

            var points = GetHyperbolicOrbitPoints.GetPoints(100, 10, Vector.Zero, 0, 3);
            points[0].X.Should().BeApproximately(412.183605, 1);
            points[0].Y.Should().BeApproximately(-39.9869134279982, 1);
            points[0].Z.Should().Be(0);
            
            points[1].X.Should().Be(100);
            points[1].Y.Should().Be(0);
            points[1].Z.Should().Be(0);
            
            points[2].X.Should().BeApproximately(412.183605, 1);
            points[2].Y.Should().BeApproximately(39.9869134279982, 1);
            points[2].Z.Should().Be(0);
        }
    }
}
