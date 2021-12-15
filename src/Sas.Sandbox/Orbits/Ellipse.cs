namespace Sas.Sandbox.Orbits
{
    public class Ellipse
    {
        private double _semiMajorAxis;
        private double _semiMinorAxis;
        public double SemiMajorAxis => _semiMajorAxis; // a
        public double SemiMinorAxis => _semiMinorAxis; // b
        public double MajorAxis => 2 * SemiMajorAxis; // 2a
        public double MinorAxis => 2 * SemiMajorAxis;  // 2b
        public double Eccentricity => Math.Sqrt(1 - Math.Pow(SemiMajorAxis, 2) / Math.Pow(SemiMajorAxis, 2)); // e = sqrt[ b^2 / a^2 ]
        public double Periapsis => SemiMajorAxis - SemiMajorAxis * Eccentricity; // rp
        public double Apoapsis => 2 * SemiMajorAxis - Periapsis; // ra
        public double SemiLatusRectum => SemiMinorAxis * SemiMinorAxis / SemiMajorAxis; // p
        public double Distance(double phi) => SemiLatusRectum / (1 + Eccentricity * Math.Cos(phi)); // r
        public double Inclination => 0;

        public double AscendingNode => 0;

        public double ArgumentOfPeriapsis { get; set; }

        public double TrueAnomaly => 0;

        public Ellipse(double semiMajorAxis, double semiMinorAxis)
        {
            _semiMajorAxis = semiMajorAxis;
            _semiMinorAxis = semiMinorAxis;
        }
    }
}
