using Sas.Body.Service.Extensions.PointExtensions;
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
        private List<Point> privatePoints;

        public string Name { get; init; }

        public List<Point> Points => points;

        public PositionedOrbit(string name, BodyDomain other, IOrbitDescription orbitDescription)
        {
            Name = name;
            OrbitDescription = orbitDescription;
            points = UpdateCenterOfPoints(other);
            privatePoints = points;
        }
        public List<Point> UpdateCenterOfPoints(BodyDomain other)
        {
            if (privatePoints is null || privatePoints.Count == 0)
            {
                privatePoints = GetOrbitPointsFactory.GetPoints(this);
            }

            Vector center = GetCenter(other);
            int count = privatePoints.Count;
            points = new List<Point>(count);
            for (int i = 0; i < count; i++)
            {
                points.Add(privatePoints[i] + center.AsPoint());
            }
            return privatePoints;
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
