﻿using Sas.Mathematica.Service.Vectors;
using Sas.Domain.Models.Orbits.Primitives;

namespace Sas.Domain.Models.Orbits
{
    public class HyperbolicOrbit : Orbit
    {
        public HyperbolicOrbit(Vector position, Vector velocity, double u) :
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
            double tanGudermannianAngle = (Math.Pow(e, 2) - 1) * Math.Sin(phi) / (1 + e * Math.Cos(phi));
            double gudermannianAngle = Math.Atan(tanGudermannianAngle);
            return Math.Log(Math.Tan(gudermannianAngle / 2 + Math.PI / 4));
        }

        public override double? GetRadius()
        {
            return null;
        }

        public override double? GetPeriod()
        {
            return null;
        }

        public override double? GetSemiMajorAxis()
        {
            return null;
        }

        public override double? GetSemiMinorAxis()
        {
            return null;
        }
    }
}
