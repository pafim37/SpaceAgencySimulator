using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.OrbitDetermination
{
    public class Observation
    {
        /// <summary>
        /// Gets observatory that create the observation
        /// </summary>
        public readonly Observatory Observatory;

        /// <summary>
        /// Gets name of the observed object
        /// </summary>
        public readonly string ObjectName;

        /// <summary>
        /// Local date of creation
        /// </summary>
        public readonly DateTime CreatedOn;

        /// <summary>
        /// Azimuth expressed in radians
        /// </summary>
        public readonly double AzimuthRad;

        /// <summary>
        /// Altitude (height) expressed in radians
        /// </summary>
        public readonly double AltitudeRad;

        /// <summary>
        /// Declination expressed in radians
        /// </summary>
        public double DeclinationRad => GetDeclinationRad();

        /// <summary>
        /// Hour angle expressed in radians
        /// </summary>
        public double HourAngleRad => GetHourAngleRad();

        /// <summary>
        /// Right Ascension expressed in radians
        /// </summary>
        public double RightAscensionRad => GetRightAscensionRad();

        /// <summary>
        /// Distance obtained by radar's method expressed in meters
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Constructor of the Observatory
        /// </summary>
        /// <param name="Observatory"></param>
        /// <param name="ObjectName"></param>
        /// <param name="createdOn"></param>
        /// <param name="azimuth"></param>
        /// <param name="altitude"></param>
        /// <param name="distance"></param>
        public Observation(Observatory observatory, string objectName, DateTime createdOn, double azimuth, double altitude, double distance)
        {
            Observatory = observatory;
            ObjectName = objectName;
            CreatedOn = createdOn;
            AzimuthRad = azimuth;
            AltitudeRad = altitude;
            Distance = distance;
        }

        private double GetHourAngleRad()
        {
            double sinT = Math.Sin(AzimuthRad + Math.PI) * Math.Cos(AltitudeRad) / Math.Cos(DeclinationRad);
            double t = Math.Asin(sinT);
            if (t < 0) t += 2 * Math.PI;
            return t;
        }

        private double GetDeclinationRad()
        {
            double azRad = AzimuthRad + Math.PI;
            double hRad = AltitudeRad;
            double fiRad = Observatory.LatitudeRad;
            double sinDec = Math.Sin(hRad) * Math.Sin(fiRad) - Math.Cos(hRad) * Math.Cos(fiRad) * Math.Cos(azRad);
            double dec = Math.Asin(sinDec);
            return dec;
        }

        private double GetRightAscensionRad()
        {
            double ra =Observatory.GetLocalSiderealTime() - HourAngleRad;
            if( ra > 2 * Math.PI)
            {
                return ra - 2 * Math.PI;
            }
            else if( ra < 0)
            {
                return ra + 2 * Math.PI;
            }
            else
            {
                return ra;
            }
        }

        //public Vector GeocentricEquatorial()
        //{
        //    double aE = Constants.EarthFlatness;
        //    double aE2 = aE * aE;
        //    double x = Constants.EarthRadius * Math.Cos(_fi) / Math.Sqrt(1 - aE2 * Math.Pow(Math.Sin(_fi), 2));
        //    double z = Constants.EarthRadius * (1 - aE2) * Math.Sin(_fi) / Math.Sqrt(1 - aE2 * Math.Pow(Math.Sin(_fi), 2));
        //    Vector R_E = new Vector(x, 0, z);
        //    return _ro * GeocetricEquatorialCosines() + R_E;
        //}

        //private Vector GeocetricEquatorialCosines()
        //{
        //    Matrix invert = TransformationMatrix().Invert();
        //    return invert * TopocentricHorizonCosines();
        //}

        //private Vector TopocentricHorizonCosines()
        //{
        //    double U = Math.Cos(_h) * Math.Sin(_a);
        //    double V = Math.Cos(_h) * Math.Cos(_a);
        //    double W = Math.Sin(_h);
        //    Console.WriteLine(U);
        //    Console.WriteLine(V);
        //    Console.WriteLine(W);
        //    return new Vector(U, V, W);
        //}

        //private Vector TopocentricEquatorial(double ro, double dec, double ra) // distance, declination, right ascension
        //{
        //    double X = ro * Math.Cos(dec) * Math.Cos(ra);
        //    double Y = ro * Math.Cos(dec) * ra * Math.Sin(ra);
        //    double Z = ro * Math.Sin(dec);
        //    return new Vector(X, Y, Z);
        //}

        //private Matrix TransformationMatrix() // for obtain U, V, W
        //{
        //    double[] r = new double[9] {-Math.Sin(_th), Math.Cos(_th), 0, -Math.Sin(_fi) *Math.Cos(_th), -Math.Sin(_fi) *Math.Sin(_th), Math.Cos(_fi), Math.Cos(_fi) *Math.Cos(_th), Math.Cos(_fi) *Math.Sin(_th), Math.Sin(_fi) };
        //    Console.WriteLine(r);
        //    return new Matrix(r);
        //}

        //private double ConvertAngleToNumber(double a)
        //{
        //    return Math.PI * a / 180;
        //}
    }
}
