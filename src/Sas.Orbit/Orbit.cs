using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Orbits
{
    public class Orbit
    {
        public int MyProperty { get; set; }

        public Orbit(Vector rVec, Vector vVec)
        {
            var h = Vector.CrossProduct(rVec, vVec); // angular momentum per unit mass

        }
    }
}
