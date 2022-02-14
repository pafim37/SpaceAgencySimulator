using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain
{
    public class AstronomicalClock
    {
        /// <summary>
        /// Gets local time
        /// </summary>
        /// <returns></returns>
        public DateTime LocalTime { get; }

        /// <summary>
        /// Gets universal time
        /// </summary>
        /// <returns></returns>
        public DateTime UniversalTime { get; }

        /// <summary>
        /// Gets local sidereal time expressed in radians
        /// </summary>
        /// <param name="lambda">Longitude of the observator in radians</param>
        /// <returns></returns>
        public double SiderealTime { get; }

        /// <summary>
        /// Constructor of the astronomical clock
        /// </summary>
        /// <param name="localtime"></param>
        /// <param name="timeZone"></param>
        public AstronomicalClock(DateTime localtime, double longitude)
        {
            _longitude = longitude;
            LocalTime = localtime;
            int timeZone = FindTimeZone(longitude);
            UniversalTime = localtime.AddHours(timeZone);
            SiderealTime = GetSiderealTimeRad();
        }

        #region provate methods
        private double GetSiderealTimeRad()
        {
            double lambda = 180 * _longitude / Math.PI; // convert to deg
            double J0 = GetJulianDate();
            double T0 = (J0 - 2451545.0) / 36525;
            double thG0 = AokisFormula(T0);
            thG0 -= 360 * (int)(thG0 / 360);
            double UT1 = UniversalTime.Hour + UniversalTime.Minute / 60.0 + UniversalTime.Second / 3600.0;
            double thGdeg = thG0 + 360.985647366 * UT1 / 24;
            thGdeg = thGdeg + lambda > 360 ? thGdeg + lambda - 360 : thGdeg + lambda;
            double thGrad = Math.PI * thGdeg / 180;
            return thGrad;
        }

        private double AokisFormula(double T0)
        {
            return 100.460618375 + 36000.7700536 * T0 + 0.000387933 * T0 * T0 - 2.5875 * Math.Pow(10, -8) * T0 * T0 * T0;
        }

        /// <summary>
        /// Gets Jefferys W. H. Julian Date Number
        /// </summary>
        /// <returns></returns>
        private double GetJulianDate()
        {
            int y = UniversalTime.Year;
            int m = UniversalTime.Month;
            if (m < 3)
            {
                y--;
                m += 12;
            }
            int d = UniversalTime.Day;
            int a = y / 100;
            int b = a / 4;
            int c = 2 - a + b;
            int e = (int)(365.25 * (y + 4716));
            int f = (int)(30.6001 * (m + 1));
            return c + d + e + f - 1524.5;
        }

        /// <summary>
        /// Find time zone using 15 degrees intervals. Can be inaccurate.
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private int FindTimeZone(double longitude)
        {
            // TODO (pafim37): This method should be more advanced
            longitude = longitude * 180 / Math.PI; // convert to deg
            return (int)((7.5 + longitude) / 15);
        }
        #endregion

        #region private fields
        /// <summary>
        /// Longitude of the clock (Observatory)
        /// </summary>
        private readonly double _longitude;
        #endregion
    }
}
