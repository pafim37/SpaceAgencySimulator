using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public class PositionedOrbit : IPositionedOrbit
    {
        public IOrbitDescription OrbitDescription { get; init; }

        private List<Point> points;

        public string Name { get; init; }

        public List<Point> Points => points;

        public PositionedOrbit(string name, BodyDomain other, IOrbitDescription orbitDescription)
        {
            Name = name;
            OrbitDescription = orbitDescription;
            points = UpdateCenterOfPoints(other);
        }
        public List<Point> UpdateCenterOfPoints(BodyDomain other)
        {
            List<Point> points = GetOrbitPointsFactory.GetPoints(this);
            Vector center = GetCenter(other);
            this.points = [.. points.Select(p => new Point(center.X + p.X, center.Y + p.Y, center.Z + p.Z))];
            return points;
        }
        private Vector GetCenter(BodyDomain other)
        {
            if (OrbitDescription.SemiMajorAxis == null)
                throw new InvalidOperationException("SemiMajorAxis cannot be null.");

            double a = OrbitDescription.SemiMajorAxis.Value;
            Vector eVec = OrbitDescription.EccentricityVector;

            return other.Position - a * eVec;
        }
    }
}
