using System;
using System.Collections.Generic;
using System.Text;

namespace Sas.Mathematica.Service.Converters
{
    public class ConvertAngles
    {
        public static double RadToDeg(double rad) => double.RadiansToDegrees(rad);

        public static double DegToRad(double degrees) => double.DegreesToRadians(degrees);
    }
}
