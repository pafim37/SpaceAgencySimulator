using Sas.Mathematica;
using Sas.Sandbox.Models;

namespace Sas.Sandbox.Orbits
{
    public class GetOrbit
    {
        public object GetOribt(Body body, Body origin)
        {
            Vector positionVector = body.GetPositionRelatedTo(origin); // r
            Vector velocityVector = body.GetVelocity(origin); // v
            Vector angularMomentumPerUnitMassVector = Vector.CrossProduct(positionVector, velocityVector); // h

            var r = positionVector.Magnitude();
            var v = velocityVector.Magnitude();
            var h = angularMomentumPerUnitMassVector.Magnitude();
            double u = Constants.G * (origin.Mass + body.Mass); // u

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
        }
    }
}
