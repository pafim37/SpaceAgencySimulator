using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.SolarSystem.Orbits
{
    public enum OrbitType
    {
        Circle,
        Ellipse,
        Parabola,
        Hyperbola,
        // TODO (pafim37): Remove non-existing orbit
        Rest
    }
}
