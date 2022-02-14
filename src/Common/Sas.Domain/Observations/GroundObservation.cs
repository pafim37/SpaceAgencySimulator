using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain.Observations
{
    public class GroundObservation : ObservationBase
    {
        /// <summary>
        /// Azimuth expressed in radians. Counted from North toward Easts
        /// </summary>
        public double AzimuthRad { get; }

        /// <summary>
        /// Altitude (height) expressed in radians
        /// </summary>
        public double AltitudeRad { get; }

        /// <summary>
        /// Hour angle expressed in radis. Counted from South towards West
        /// </summary>
        public double HourAngleRad { get; }

        /// <summary>
        /// Declination expressed in radians
        /// </summary>
        public double DeclinationRad { get; }

        /// <summary>
        /// Right Ascension expressed in radians
        /// </summary>
        public double RightAscensionRad { get; }


        /// <summary>
        /// Constructor of the Ground Observation
        /// </summary>
        /// <param name="observatory"></param>
        /// <param name="objectName"></param>
        /// <param name="createdOn"></param>
        /// <param name="azimuth"></param>
        /// <param name="altitude"></param>
        public GroundObservation(
            Observatory observatory,
            string objectName,
            DateTime createdOn,
            double azimuth,
            double altitude
            )
            : base(observatory, objectName, createdOn)
        {
            AzimuthRad = azimuth;
            AltitudeRad = altitude;
            HourAngleRad = GetHourAngleRad();
            DeclinationRad = GetDeclinationRad();
            RightAscensionRad = GetRightAscensionRad();
        }

        #region private methods

        private double GetHourAngleRad()
        {
            double a = AzimuthRad;
            double h = AltitudeRad;
            double dec = DeclinationRad;
            double sinT = -Math.Sin(a) * Math.Cos(h) / Math.Cos(dec);
            double t = Math.Asin(sinT);
            return t < 0 ? t += 2 * Math.PI : t;
        }

        private double GetDeclinationRad()
        {
            double a = AzimuthRad;
            double h = AltitudeRad;
            double fi = Observatory.LatitudeRad;
            double sinDec = Math.Sin(h) * Math.Sin(fi) + Math.Cos(h) * Math.Cos(fi) * Math.Cos(a);
            double dec = Math.Asin(sinDec);
            return dec;
        }

        private double GetRightAscensionRad()
        {
            double ra = Time.SiderealTime - HourAngleRad;
            return ra > 0 ? ra : ra + 2 * Math.PI;
        }

        #endregion
    }
}
