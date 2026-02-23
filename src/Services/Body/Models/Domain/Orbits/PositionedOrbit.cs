using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public class PositionedOrbit(string name, IOrbitDescription orbitDescription) : IPositionedOrbit
    {
        public IOrbitDescription OrbitDescription { get; init; } = orbitDescription;
        public string Name { get; init; } = name;
        public Vector? Center { get; set; }
        public List<Point>? Points { get; set; }
    }
}
