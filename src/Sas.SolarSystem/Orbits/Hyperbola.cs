using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.BodySystem.Orbits
{
    internal class Hyperbola : Orbit
    {
        public Hyperbola(Vector positionRelated, Vector velocityRelated, double u) : base(positionRelated, velocityRelated, u)
        {
        }
    }
}
