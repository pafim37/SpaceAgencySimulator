using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public interface IPositionedOrbit
    {
        public IOrbitDescription? OrbitDescription { get; set; }
        public List<Point>? Points { get; set; }
        public Vector? Center { get; set; }
    }
}
