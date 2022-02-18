using Sas.Mathematica;

namespace Sas.SolarSystem.Service.DTOs
{
    public class BodyDTO
    {
        public string Name { get; set; }
        public double Mass { get; set; }
        public Vector AbsolutePosition { get; set; }
        public Vector AbsoluteVelocity { get; set; }
    }
}
