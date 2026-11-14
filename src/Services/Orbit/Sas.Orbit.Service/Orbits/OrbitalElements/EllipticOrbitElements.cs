using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.Primitives;

namespace Sas.Orbit.Service.Orbits.OrbitalElements
{
    public class EllipticOrbitElements : OrbitElements
    {
        public EllipticOrbitElements(Vector position, Vector velocity, double u) :
            base(position, velocity, u)
        {
            _type = OrbitType.Elliptic;
        }

        protected override double GetMeanAnomaly(double e, double ae)
        {
            return ae - e * Math.Sin(ae);
        }

        protected override double GetEccentricAnomaly(double e, double phi)
        {
            double cosAE = (e + Math.Cos(phi)) / (1 + e * Math.Cos(phi));
            return Math.Acos(cosAE);
        }

        protected override double? GetRadius()
        {
            return null;
        }

        protected override double? GetPeriod()
        {
            return _period;
        }

        protected override double? GetSemiMajorAxis()
        {
            return _a;
        }

        protected override double? GetSemiMinorAxis()
        {
            return _b;
        }
    }
}
