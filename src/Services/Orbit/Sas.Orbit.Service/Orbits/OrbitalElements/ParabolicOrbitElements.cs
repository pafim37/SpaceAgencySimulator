using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.Primitives;

namespace Sas.Orbit.Service.Orbits.OrbitalElements
{
    public class ParabolicOrbitElements : OrbitElements
    {
        public ParabolicOrbitElements(Vector position, Vector velocity, double u) :
            base(position, velocity, u)
        {
            _type = OrbitType.Parabolic;
        }

        protected override double GetMeanAnomaly(double e, double ae)
        {
            return e * Math.Sinh(ae) - ae;
        }

        protected override double GetEccentricAnomaly(double e, double phi)
        {
            double tanGudermannianAngle = (Math.Pow(e, 2) - 1) * Math.Sin(phi) / (1 + e * Math.Cos(phi));
            double gudermannianAngle = Math.Atan(tanGudermannianAngle);
            return Math.Log(Math.Tan(gudermannianAngle / 2 + Math.PI / 4));
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
            return null;
        }

        protected override double? GetSemiMinorAxis()
        {
            return null;
        }
    }
}
