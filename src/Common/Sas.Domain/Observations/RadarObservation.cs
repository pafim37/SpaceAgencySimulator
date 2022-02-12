namespace Sas.Domain.Observations
{
    public class RadarObservation : GroundObservation
    {
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
