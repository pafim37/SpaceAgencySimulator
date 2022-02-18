using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain.Bodies
{
    public class CelestialBody : BodyBase
    {
        /// <summary>
        /// Radius of the body
        /// </summary>
        public double Radius { get; }
        public CelestialBody(string name, double mass, Vector position, Vector velocity, double radius) 
            : base(name, mass, position, velocity)
        {
            Radius = radius;
        }
    }
}
