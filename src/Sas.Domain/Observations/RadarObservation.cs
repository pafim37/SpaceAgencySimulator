using Sas.Domain.Observatories;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Observations
{
    public class RadarObservation : GroundObservation
    {
        /// <summary>
        /// Distance obtained by radar's method expressed in meters
        /// </summary>
        public double Distance { get; }

        /// <summary>
        /// A referece system on the surface of the Earth where the observer is located
        /// U - pointing towards east
        /// V - pointing towards north
        /// W - pointing towards zenith
        /// That referece system rotating with Earth
        /// </summary>
        /// <returns></returns>
        public Vector TopocentricHorizonSystemCoordinate { get; }

        /// <summary>
        /// A reference system on the surface of the Earth where the observer is located
        /// Xt - pointing towards vernal equinox
        /// Yt - pointing towards celestial pole
        /// Zt - pointing towards Yt x Xt
        /// </summary>
        /// <returns></returns>
        public Vector TopocentricEquatorialSystemCoordinate { get; }

        /// <summary>
        /// A reference system in center of the Earth
        /// X - pointing towards vernal equinox
        /// Y - pointing towards celestial pole
        /// Z - pointing towards Yt x Xt
        /// </summary>
        /// <returns></returns>
        public Vector GeocentricEquatorialSystemCoordinate { get; }

        /// <summary>
        /// Constructor of the Observatory
        /// </summary>
        /// <param name="Observatory"></param>
        /// <param name="ObjectName"></param>
        /// <param name="createdOn"></param>
        /// <param name="azimuth"></param>
        /// <param name="altitude"></param>
        /// <param name="distance"></param>
        public RadarObservation(
            Observatory observatory,
            string objectName,
            DateTime createdOn,
            double azimuth,
            double altitude,
            double distance
            )
            : base(observatory, objectName, createdOn, azimuth, altitude)
        {
            Distance = distance;
            TopocentricHorizonSystemCoordinate = GetTopocentricHorizonSystemCoordinate();
            TopocentricEquatorialSystemCoordinate = GetTopocentricEquatorialSystemCoordinate();
            GeocentricEquatorialSystemCoordinate = GetGeocentricEquatorialSystemCoordinate();
        }

        #region private methods
        private Vector GetTopocentricHorizonSystemCoordinate()
        {
            return Vector.Zero; // TransformationMatrix() * GetTopocentricEquatorialSystemCoordinate();
        }

        private Vector GetTopocentricEquatorialSystemCoordinate()
        {
            double ro = Distance;
            double ra = RightAscensionRad;
            double dec = DeclinationRad;
            double Xt = Distance * Math.Cos(dec) * Math.Cos(ra);
            double Yt = ro * Math.Cos(dec) * ra * Math.Sin(ra);
            double Zt = ro * Math.Sin(dec);
            return new Vector(Xt, Yt, Zt);
        }
        
        private Vector GetGeocentricEquatorialSystemCoordinate()
        {
            double fi = Observatory.LatitudeRad;
            double aE = Constants.EarthRadius;
            double eE = Constants.EarthFlatness;
            double eE2 = eE * eE;
            double x = aE * Math.Cos(fi) / Math.Sqrt(1 - eE2 * Math.Pow(Math.Sin(fi), 2)); // taking the flatness of the Earth into account
            double z = aE * (1 - eE2) * Math.Sin(fi) / Math.Sqrt(1 - eE2 * Math.Pow(Math.Sin(fi), 2)); // taking the flatness of the Earth into account
            double xH = x + Observatory.Height * Math.Cos(fi); // taking the height of the observer into account  
            double zH = z + Observatory.Height * Math.Sin(fi); // taking the height of the observer into account  

            double th = Time.SiderealTime;
            Vector rE = new Vector(xH * Math.Cos(th), xH * Math.Sin(th), zH);
            return rE;
        }

        /// <summary>
        /// Rotation matrix from topocentric equatorial system to topocentric horizon system
        /// </summary>
        /// <returns></returns>
        private Matrix TransformationMatrix() // for obtain U, V, W from X Y Z
        {
            double fi = Observatory.LatitudeRad;
            double th = Observatory.LongitudeRad;
            double[] r = new double[9] { -Math.Sin(th), Math.Cos(th), 0, -Math.Sin(fi) * Math.Cos(th), -Math.Sin(fi) * Math.Sin(th), Math.Cos(fi), Math.Cos(fi) * Math.Cos(th), Math.Cos(fi) * Math.Sin(th), Math.Sin(fi) };
            return new Matrix(r, 3, 3);
        }
        #endregion
    }
}
