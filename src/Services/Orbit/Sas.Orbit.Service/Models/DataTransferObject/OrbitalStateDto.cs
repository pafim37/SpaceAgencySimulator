using Sas.Mathematica.Service.Vectors;

namespace Sas.Orbit.Service.Models.DataTransferObject
{
    public record OrbitalStateDto
    {
        public Vector? Position { get; init; }
        public Vector? Velocity { get; init; }
        public double GravitationalParameter { get; init; }
    }
}
