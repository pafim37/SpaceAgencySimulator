using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public class OrbitHolder
    {
        public string? Name { get; set; }
        public Vector? Center { get; set; }
        public Orbit? Orbit { get; set; }
        public double Rotation { get; set; }
    }
}
