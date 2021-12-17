using Sas.Mathematica;

namespace Sas.SolarSystem.Orbits
{
    public class Orbit
    {
        protected double _a; // semi major axis
        protected double _b; // semi minor axis
        protected double _r; // distance 
        protected double _v; // velocity
        protected double _h; // angular momentum per unit mass
        protected double _p; // semi latus rectum
        protected double _e; // eccentricy
        protected double _u; // G * (M + m)
        protected OrbitType _type; // orbit type: circle, ellipse, parabola, hyperbola
        protected double _th; // true anomaly
        protected double _w;  // argument of periapsis
        protected double _i;  // inclination 
        protected double _om; // ascending node

        /// <summary>
        /// Semi major axis
        /// </summary>
        public double SemiMajorAxis => _a;
        
        /// <summary>
        /// Semi minor axis
        /// </summary>
        public double SemiMinorAxis => _b;

        /// <summary>
        /// Distance from focus
        /// </summary>
        public double DistanceFromFocus => _r;
        
        /// <summary>
        /// Velocity on the orbit
        /// </summary>
        public double Velocity => _v;

        /// <summary>
        /// Angular momentum per unit mass
        /// </summary>
        public double AngularMomentumPerUnitMass => _h;
        
        /// <summary>
        /// Semi latus rectum
        /// </summary>
        public double SemiLatusRectum => _p;

        /// <summary>
        /// Eccentricity
        /// </summary>
        public double Eccentricity => _e; // e 

        /// <summary>
        /// G * (M + m)
        /// </summary>
        public double U => _u;

        /// <summary>
        /// Orbit type
        /// </summary>
        public OrbitType OrbitType => _type;

        /// <summary>
        /// True anomaly
        /// </summary>
        public double TrueAnomaly => _th;

        /// <summary>
        /// Argument of periapsis
        /// </summary>
        public double ArgumentOfPeriApsis => _w;

        /// <summary>
        /// Inclination
        /// </summary>
        public double Inclination => _i;

        /// <summary>
        /// Ascending node
        /// </summary>
        public double AscendingNode { get; set; }

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
            Vector hVector = Vector.CrossProduct(positionRelated, velocityRelated);
            Vector eVector = 1 / u * Vector.CrossProduct(velocityRelated, hVector) - r * positionRelated;
            double h = hVector.Magnitude();
            double e = Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));
            
            _r = r;
            _v = v;
            _u = u;
            _h = h;
            _e = e;
            _p = h * h / u; 
            _a = 1 / (2 / r - v * v / u);
            _b = _p / Math.Sqrt(1 - e * e);
            _type = GetOrbitType(e);
            _w = Math.Acos(Vector.DotProduct(velocityRelated, eVector) / (e * r)); 
        }

        /// <summary>
        /// Distance in polar coordinate system
        /// </summary>
        /// <param name="trueAnomaly">true anomaly</param>
        /// <returns></returns>
        public double GetR(double th) => _h * _h / _u / (1 + Math.Cos(th)); // r = p / ( 1 + cos(fi))

        /// <summary>
        /// Orbital velocity
        /// </summary>
        /// <param name="trueAnomaly"></param>
        /// <returns></returns>
        public double GetV(double th) => _h / Math.Pow(GetR(th), 2);

        public override string? ToString()
        {
            return $"e: {_e}, type: {_type}";
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
        public static Orbit? CreateOrbit(Vector positionVector, Vector velocityVector, double u)
        {
            var r = positionVector;
            var v = velocityVector;
            
            OrbitType type = GetOrbitType(r, v, u);

            if (type == OrbitType.Circle) return new Circle(r, v, u);
            else if (type == OrbitType.Ellipse) return new Ellipse(r, v, u);
            else if (type == OrbitType.Parabola) return new Parabola(r, v, u);
            else if (type == OrbitType.Hyperbola) return new Hyperbola(r, v, u);

            else return null;
        }
    }
}
