using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public interface IPositionedOrbit
    {
        public IOrbitDescription OrbitDescription { get; init; }
        public string Name { get; init; }
        public double Phi { get; init; }
        public double Theta { get; init; }
        public double Eta { get; init; }
        public Vector? Center { get; set; }
        public List<Point>? Points { get; set; }
    }
}
