using Sas.Mathematica;

namespace Sas.Domain.Bodies
{
    public class CelestialBody : BodyBase
    {
        /// <summary>
        /// Radius of the celeastial body
        /// </summary>
        public double Radius { get; set; }

        public CelestialBody(string name, double mass, Vector position, Vector velocity) 
            : base(name, mass, position, velocity)
        {
        }

        public CelestialBody() : base()
        {
        }
    }
}
