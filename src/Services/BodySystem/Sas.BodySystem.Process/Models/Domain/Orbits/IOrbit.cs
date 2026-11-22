using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;
using Sas.Orbit.Service.Orbits.Points;

namespace Sas.BodySystem.Process.Models.Domain.Orbits
{
    public interface IOrbit
    {
        public IOrbitElements OrbitElements { get; init; }
        public string Name { get; init; }
        public List<Point> Points { get; }
        public List<Point> UpdateCenterOfPoints(Vector gravitationalSourcePosition);
    }
}
