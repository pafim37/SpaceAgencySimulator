using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public interface IPositionedOrbit
    {
        public IOrbitDescription OrbitDescription { get; init; }
        public string Name { get; init; }
        public List<Point> Points { get; }
        public List<Point> UpdateCenterOfPoints(BodyDomain other);
    }
}
