using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions
{
    public interface IOrbitDescription
    {
        public OrbitType OrbitType { get; }
        public double? SemiMajorAxis { get; }
        public double? SemiMinorAxis { get; }
        public double SemiLatusRectum { get; }
        public double Eccentricity { get; }
        public double ArgumentOfPeriapsis { get; }
        public double Inclination { get; }
        public double AscendingNode { get; }
        public double TrueAnomaly { get; }
        public double EccentricAnomaly { get; }
        public Vector EccentricityVector { get; }
        public double MeanAnomaly { get; }
        public double? Period { get; }
        public double? Radius { get; }
    }
}


