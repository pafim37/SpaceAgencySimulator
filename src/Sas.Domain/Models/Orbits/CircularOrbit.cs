﻿using Sas.Mathematica.Service.Vectors;
using Sas.Domain.Models.Orbits.Primitives;

namespace Sas.Domain.Models.Orbits
{
    public class CircularOrbit : Orbit
    {
        public CircularOrbit(Vector position, Vector velocity, double u) :
            base(position, velocity, u)
        {
            _type = OrbitType.Circular;
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
            return _radius;
        }

        protected override double? GetPeriod()
        {
            return _period;
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
