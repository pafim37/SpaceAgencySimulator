using Sas.Mathematica;

namespace Sas.SolarSystem.Orbits
{
    public class Orbit
    {
        protected double _r; // distance 
        protected double _v; // velocity
        protected double _h; // angular momentum per unit mass

        protected double _p; // semi latus rectum
        public double SemiLatusRectum => _p;

        protected double _e; // eccentricy
        public double Eccentricity => _e; // e 

        protected double _u; // G (M + m)

        protected OrbitType _type; // orbit type: circle, ellipse, parabola, hyperbola
        public OrbitType OrbitType => _type;

        protected double _th; // true anomaly
        protected double _w;  // argument of periapsis
        //    Vector eVector = 1 / u * Vector.CrossProduct(velocityVector, angularMomentumPerUnitMassVector) - r * positionVector;
        //    double w = Math.Acos(Vector.DotProduct(velocityVector, eVector) / (e * r)); // argument of pericentrum
        protected double _i;  // inclination 
        protected double _om; // ascending node

        /// <summary>
        /// Constructor of the orbit
        /// </summary>
        /// <param name="position">position related to the mass center</param>
        /// <param name="velocity">velocity related to the mass center</param>
        /// <param name="u"></param>
        public Orbit(Vector positionRelated, Vector velocityRelated, double u)
        {
            double r = positionRelated.Magnitude();
            double v = velocityRelated.Magnitude();
            _r = r;
            _v = v;
            _u = u;
            double h = Vector.CrossProduct(positionRelated, velocityRelated).Magnitude();
            _h = h;
            double e = Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));
            _e = e;
            _type = GetOrbitType(e);
        }

        /// <summary>
        /// Distance in polar coordinate system
        /// </summary>
        /// <param name="trueAnomaly">true anomaly</param>
        /// <returns></returns>
        public double GetR(double trueAnomaly) => _h * _h / _u / (1 + Math.Cos(trueAnomaly)); // r = p / ( 1 + cos(fi))

        /// <summary>
        /// Orbital velocity
        /// </summary>
        /// <param name="trueAnomaly"></param>
        /// <returns></returns>
        public double GetV(double trueAnomaly) => _h / Math.Pow(GetR(trueAnomaly), 2);

        public override string? ToString()
        {
            return $"\ne: {_e}, h: {_h}";
        }

        public static OrbitType GetOrbitType(Vector pos, Vector vel, double u)
        {
            double r = pos.Magnitude();
            double v = vel.Magnitude();
            double h = Vector.CrossProduct(pos, vel).Magnitude();
            double e2 = 1 + v * v * h * h / (u * u) - 2 * h * h / (u * r);
            double e = Math.Sqrt(e2);
            return Orbit.GetOrbitType(e);
        }

        private static OrbitType GetOrbitType(double e)
        {
            if (e == 0) return OrbitType.Circle;
            else if (e > 0 && e <= 1) return OrbitType.Ellipse;
            else if (e > 1) return OrbitType.Hyperbola;
            else if (e == 1) return OrbitType.Parabola;
            else return OrbitType.Rest;

        }
        public static Orbit CreateOrbit(Vector positionVector, Vector velocityVector, double u)
        {
            var r = positionVector;
            var v = velocityVector;
            OrbitType type = GetOrbitType(r, v, u);
            if (type == OrbitType.Circle) return new Circle(r, v, u);
            else if (type == OrbitType.Ellipse) return new Ellipse(r, v, u);
            else if (type == OrbitType.Parabola) return new Parabola(r, v, u);
            else if (type == OrbitType.Hyperbola) return new Hyperbola(r, v, u);
            else
            {
                Console.WriteLine("Uwaga zwracam nulla");
                return null;
            }
        }
    }
}
