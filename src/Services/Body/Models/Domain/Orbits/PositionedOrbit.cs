using Sas.Body.Service.Models.Domain.Orbits.OrbitInfos;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public class PositionedOrbit
    {
        public OrbitDescription? OrbitDescription { get; set; }
        public List<Point>? Points { get; set; }
        public Vector? Center { get; set; }
    }
}
