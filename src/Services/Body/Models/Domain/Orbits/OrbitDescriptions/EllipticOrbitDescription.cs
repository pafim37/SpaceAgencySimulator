using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.OrbitInfos
{
    public class EllipticOrbitDescription : OrbitDescription
    {
        public EllipticOrbitDescription(Vector position, Vector velocity, double u) :
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
