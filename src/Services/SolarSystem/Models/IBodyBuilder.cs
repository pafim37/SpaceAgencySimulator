using Sas.Mathematica.Service.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.SolarSystem.Service.Models
{
    internal interface IBodyBuilder
    {
        Body SetName(string name);
        Body SetMass(double mass);
        Body SetRadius(double radius);
        Body SetPosition(Vector position);
        Body SetVelocity(Vector velocity);
    }
}
