using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.Primitives;

namespace Sas.Orbit.Service.Orbits.OrbitalElements
{
    public class HyperbolicOrbitElements : OrbitElements
    {
        public HyperbolicOrbitElements(Vector position, Vector velocity, double u) :
            base(position, velocity, u)
        {
            _type = OrbitType.Hyperbolic;
        }

        protected override double GetMeanAnomaly(double e, double ae)
        {
            return e * Math.Sinh(ae) - ae;
        }

        protected override double GetEccentricAnomaly(double e, double phi)
        {
            double sinhAnomaly = Math.Sqrt((e - 1) * (e + 1)) * Math.Sin(phi) / (1 + e * Math.Cos(phi));
            return Math.Asinh(sinhAnomaly);
        }

        protected override double? GetRadius()
        {
            return null;
        }

        protected override double? GetPeriod()
        {
            return null;
        }

        protected override double? GetSemiMajorAxis()
        {
            return _a;
        }

        protected override double? GetSemiMinorAxis()
        {
            return _a * Math.Sqrt(_e * _e - 1);
        }
    }
}
