using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;
using Sas.Orbit.Service.Orbits.Points;
using Sas.Orbit.Service.PointExtensions;

namespace Sas.Orbit.Service.Orbits
{
    public class PositionedOrbit : IPositionedOrbit
    {
        private List<Point> points;
        public string Name { get; init; }
        public IOrbitElements OrbitElements { get; init; }

        public List<Point> Points => points;

        public PositionedOrbit(string name, Vector gravitationalSourcePosition, IOrbitElements orbitElements)
        {
            Name = name;
            OrbitElements = orbitElements;
            points = UpdateCenterOfPoints(gravitationalSourcePosition);
        }
        public List<Point> UpdateCenterOfPoints(Vector gravitationalSourcePosition)
        {
            if (points is null || points.Count == 0)
            {
                points = GetOrbitPointsFactory.GetPoints(OrbitElements);
            }

            if (points.Count == 0)
            {
                return points;
            }

            Vector center = CalculateOrbitCenter(gravitationalSourcePosition);
            int count = points.Count;
            List<Point> translatedPoints = new(count);
            for (int i = 0; i < count; i++)
            {
                translatedPoints.Add(points[i] + center.AsPoint());
            }
            points = translatedPoints;
            return points;
        }
        private Vector CalculateOrbitCenter(Vector gravitationalSourcePosition)
        {
            if (OrbitElements.SemiMajorAxis == null)
            {
                throw new InvalidOperationException("SemiMajorAxis cannot be null when calculating the center of the orbit.");
            }

            double a = OrbitElements.SemiMajorAxis.Value;
            Vector eVec = OrbitElements.EccentricityVector;

            return gravitationalSourcePosition - a * eVec;
        }
    }
}