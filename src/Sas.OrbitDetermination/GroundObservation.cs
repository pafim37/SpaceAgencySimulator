using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain
{
    public class GroundObservation : ObservationBase
    {
        /// <summary>
        /// Azimuth expressed in radians
        /// </summary>
        public double AzimuthRad { get; set; }

        /// <summary>
        /// Altitude (height) expressed in radians
        /// </summary>
        public double AltitudeRad { get; set; }

        /// <summary>
        /// Hour angle expressed in radis calculated from South
        /// </summary>
        public double HourAngleRad => GetHourAngleRad();
        
        /// <summary>
        /// Declination expressed in radians
        /// </summary>
        public double DeclinationRad => GetDeclinationRad();

        /// <summary>
        /// Right Ascension expressed in radians
        /// </summary>
        public double RightAscensionRad => GetRightAscensionRad();


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
        }

        #region private methods

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
            double ra = Observatory.GetLocalSiderealTime() - HourAngleRad;
            if (ra > 2 * Math.PI)
            {
                return ra - 2 * Math.PI;
            }
            else if (ra < 0)
            {
                return ra + 2 * Math.PI;
            }
            else
            {
                return ra;
            }
        }

        #endregion
    }
}
