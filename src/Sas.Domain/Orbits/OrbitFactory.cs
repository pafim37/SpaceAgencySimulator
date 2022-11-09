using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Orbits
{
    public static class OrbitFactory
    {
        public static Orbit Factory(Vector position, Vector velocity, double u) 
        {
            double r = position.Magnitude;
            double v = velocity.Magnitude;
            Vector hVector = Vector.CrossProduct(position, velocity);
            Vector eVector = 1 / u * Vector.CrossProduct(velocity, hVector) - 1 / r * position;
            //double e = Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));
            double e = eVector.Magnitude;
            Vector nVector = new Vector(-hVector.Y, hVector.X, 0); //first node vector n
            double n = nVector.Magnitude;
            double a = 1 / (2 / r - v * v / u);
            if (e < 0) return new Orbit(position, velocity, u);
            else if (e > 0 && e < 1 ) return new Orbit(position, velocity, u);
            else if (e == 1) return new Orbit(position, velocity, u);
            else if (e > 1) return new Orbit(position, velocity, u);
            else
            {
                throw new Exception();
            }
        }
    }
}
