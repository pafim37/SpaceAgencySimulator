using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.SolarSystem.Orbits
{
    internal class Parabola : Orbit
    {
        public Parabola(Vector positionRelated, Vector velocityRelated, double u) : base(positionRelated, velocityRelated, u)
        {
        }
    }
}
