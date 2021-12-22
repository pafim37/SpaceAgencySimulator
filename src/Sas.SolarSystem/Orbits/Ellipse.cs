using Sas.Mathematica;

namespace Sas.SolarSystem.Orbits
{
    public class Ellipse : Orbit
    {
        private double _M; // mean anomaly
        private double _AE; // eccentric anomaly

        /// <summary>
        /// Mean anomaly
        /// </summary>
        public double MeanAnomaly => _M;

        /// <summary>
        /// Eccentric anomaly
        /// </summary>
        public double EccentricAnomaly => _AE;

        public Ellipse(Vector positionRelated, Vector velocityRelated, double u) : base(positionRelated, velocityRelated, u)
        {
            double CosM = ( _e + Math.Cos(_th) ) / (1 + _e * Math.Cos(_th));
            _M = Math.Acos( CosM );
            _AE = 2 * Math.Atan( Math.Tan(_th / 2) * Math.Sqrt( (1 - _e) / ( 1 + _e) ) );
        }
    }
}
