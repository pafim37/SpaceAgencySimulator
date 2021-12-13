using Sas.Mathematica.Models;
using Sas.Orbit.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Orbit.Orbits
{
    public class GetOrbit
    {
        public object GetOribt(Body body, Body origin)
        {
            Vector positionVector = body.GetPosition(origin); // r
            Vector velocityVector = body.GetVelocity(origin); // v
            Vector angularMomentumPerUnitMassVector = Vector.CrossProduct(positionVector, velocityVector); // h
            
            var r = positionVector.Magnitude();
            var v = velocityVector.Magnitude();
            var h = angularMomentumPerUnitMassVector.Magnitude();
            double u = Physics.Constants.G * (origin.Mass + body.Mass); // u

            double e2 = 1 + Math.Pow(v * h / u, 2) - 2 * Math.Pow(h, 2) / (u * r); // e^2  = 1 + (vh/u)^2 - 2h^2/u/r 
            double e = Math.Sqrt(e2);

            Vector eVector = 1 / u * Vector.CrossProduct(velocityVector, angularMomentumPerUnitMassVector) - r * positionVector;
            double w = Math.Acos(Vector.DotProduct(velocityVector, eVector) / (e * r)); // argument of pericentrum

            if (e == 0)
            {
                // circle
                return null;
            }
            else if (e > 0 && e < 1)
            {
                double p = h * h / u;
                double a = 1 / (2 / r - v * v / u);
                double b = p / Math.Sqrt(1 - e * e);
                Ellipse ellipse = new Ellipse(a, b);
                ellipse.ArgumentOfPeriapsis = w;
                return ellipse;
            }
            else if (e == 1)
            {
                // parabolic 
                return null;
            }
            else
            {
                // hiperbolic
                return null;
            }

            //double p = h * h / u;

            //double a = 1 / (2 / r - v * v / u);
            //double b = p / Math.Sqrt(1 - e * e);
            //Console.WriteLine($"a: {a}");
            //Console.WriteLine($"b: {b}");
            //double rp = a - a * e;
            //double ra = 2 * a - rp;

            //double CosTh = h * h / (u * e * rp) - 1 / e;
            //Console.WriteLine($"1: {h * h / (u * e * rp)}");
            //Console.WriteLine($"2: {1 / e}");
            //Console.WriteLine($"CosTh: {CosTh}");
            //double th;
            //if(CosTh >= 1)
            //{
            //    th = Math.PI;
            //}
            //else if (CosTh <= -1)
            //{
            //    th = 0;
            //}
            //else
            //{
            //    th = Math.Acos(CosTh);
            //}

            //double v2 = u * (2 / r - 1 / a);
            //Console.WriteLine($"rp: {rp}");
            //Console.WriteLine($"ra: {ra}");
            //Console.WriteLine($"th: {th * 180 / Math.PI} ");

            //return e;
        }
    }
}
