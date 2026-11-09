using Sas.Mathematica.Service.Vectors;

namespace Sas.Orbit.Service.Models.DataTransferObject
{
    public record TwoBodyState
    {
        public string? Name { get; init; }
        public Vector? Position { get; init; }
        public Vector? Velocity { get; init; }
        public Vector? GravitationalSourcePosition { get; init; }
        public double GravitationalParameter { get; init; }

    }
}
