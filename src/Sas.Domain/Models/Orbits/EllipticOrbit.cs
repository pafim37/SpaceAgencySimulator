using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Orbits
{
    public class EllipticOrbit : Orbit
    {
        public EllipticOrbit(Vector position, Vector velocity, double u) :
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
