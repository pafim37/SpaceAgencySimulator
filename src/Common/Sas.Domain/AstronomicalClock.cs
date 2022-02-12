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
        /// Mean solar time of the Greenwich
        /// </summary>
        private readonly DateTime _universaltime;

        /// <summary>
        /// Time zone -12 <= t <= 12
        /// </summary>
        private readonly int _timeZone;

        /// <summary>
        /// Constructor of the astronomical clock
        /// </summary>
        /// <param name="localtime"></param>
        /// <param name="timeZone"></param>
        public AstronomicalClock(DateTime localtime, int timeZone)
        {
            if (timeZone < 12 && timeZone > -12)
            {
                _timeZone = timeZone;
                _universaltime = localtime.AddHours(-timeZone);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Time zone out of the range");
            }
        }

        /// <summary>
        /// Gets local time
        /// </summary>
        /// <returns></returns>
        public DateTime GetLocalTime() => _universaltime.AddHours(_timeZone);

        /// <summary>
        /// Gets universal time
        /// </summary>
        /// <returns></returns>
        public DateTime GetUniversalTime() => _universaltime;

        /// <summary>
        /// Gets local sidereal time expressed in radians
        /// </summary>
        /// <param name="lambda">Longitude of the observator in radians</param>
        /// <returns></returns>
        public double GetLocalSiderealTime(double lambda)
        {
            return SiderealTimeRad(lambda);
        }

        /// <summary>
        /// Gets Jefferys W. H. Julian Date Number
        /// </summary>
        /// <returns></returns>
        public double GetJulianDate()
        {
            int y = _universaltime.Year;
            int m = _universaltime.Month;
            if (m < 3)
            {
                y--;
                m += 12;
            }
            int d = _universaltime.Day;
            int a = y / 100;
            int b = a / 4;
            int c = 2 - a + b;
            int e = (int)(365.25 * (y + 4716));
            int f = (int)(30.6001 * (m + 1));
            return c + d + e + f - 1524.5;
        }

        /// <summary>
        /// Return sidereal time as deg 
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private double SiderealTimeRad(double lambda)
        {
            lambda = 180 * lambda / Math.PI; // convert to deg
            double J0 = GetJulianDate();
            double T0 = (J0 - 2451545.0) / 36525;
            double thG0 = 100.460618375 + 36000.7700536 * T0 + 0.000387933 * T0 * T0 - 2.5875 * Math.Pow(10, -8) * T0 * T0 * T0;
            thG0 -= 360 * (int)(thG0 / 360);
            double UT1 = _universaltime.Hour + _universaltime.Minute / 60.0 + _universaltime.Second / 3600.0;
            double thGdeg = thG0 + 360.985647366 * UT1 / 24;
            thGdeg = thGdeg + lambda > 360 ? thGdeg + lambda - 360 : thGdeg + lambda;
            double thGrad = Math.PI * thGdeg / 180;
            return thGrad;
        }
    }
}
