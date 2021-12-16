using Sas.Mathematica;

namespace Sas.SolarSystem.Orbits
{
    public class Ellipse : Orbit
    {
        private double _semiMajorAxis;
        private double _semiMinorAxis;
        public double SemiMajorAxis => _semiMajorAxis; // a
        public double SemiMinorAxis => _semiMinorAxis; // b
        public double MajorAxis => 2 * _semiMajorAxis; // 2a
        public double MinorAxis => 2 * _semiMajorAxis;  // 2b
        public double Periapsis => _semiMajorAxis - _semiMajorAxis * _e; // rp
        public double Apoapsis => 2 * _semiMajorAxis - Periapsis; // ra

        public Ellipse(Vector positionRelated, Vector velocityRelated, double u) : base(positionRelated, velocityRelated, u)
        {
            double _semiMajorAxis = 1 / (2 / _r - _v * _v / _u);
            double _semiMinorAxis = _p / Math.Sqrt(1 - _e * _e);
        }
    }
}
