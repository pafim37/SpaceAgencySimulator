﻿using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions
{
    public class HyperbolicOrbitDescription : OrbitDescription
    {
        public HyperbolicOrbitDescription(Vector position, Vector velocity, double u) :
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
