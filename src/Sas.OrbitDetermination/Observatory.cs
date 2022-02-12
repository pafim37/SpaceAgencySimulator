namespace Sas.OrbitDetermination
{
    public class Observatory
    {
        /// <summary>
        /// Local clock of the observatory
        /// </summary>
        private AstronomicalClock _time;

        /// <summary>
        /// Latitude of the observatory normalized to 1 
        /// </summary>
        public double LatitudeRad { get; } // latitude

        /// <summary>
        /// Longitude of the observatory normalized to 1
        /// </summary>
        public double LongitudeRad { get; } // longitude

        /// <summary>
        /// Name of the observatory
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructor of the observatory class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public Observatory(string name, double latitude, double longitude)
        {
            if (IsValidate(latitude) && IsValidate(longitude))
            {
                Name = name;
                LatitudeRad = latitude;
                LongitudeRad = longitude;

                int timeZone = FindTimeZone(longitude);
                _time = new AstronomicalClock(DateTime.Now, timeZone);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Latitude or Longitude out of range exception");
            }
        }

        /// <summary>
        /// Gets local sidereal time for observatory
        /// </summary>
        /// <returns></returns>
        public double GetLocalSiderealTime()
        {
            return _time.GetLocalSiderealTime(LongitudeRad);
        }
        
        // TODO (pafim37): This method should be removed - created for tests
        public void SetLocalTime(DateTime localtime, int timeZone)
        {
            _time = new AstronomicalClock(localtime, timeZone);
        }

        /// <summary>
        /// Create observation
        /// </summary>
        /// <param name="objectName">What is observed as a string</param>
        /// <param name="azimuth"></param>
        /// <param name="altitude"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Observation CreateObservation(string objectName, double azimuth, double altitude, double distance)
        {
            return new Observation(this, objectName, _time.GetLocalTime(), azimuth, altitude, distance);
        }

        /// <summary>
        /// Find time zone using 15 degrees intervals. Can be inaccurate.
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private int FindTimeZone(double longitude)
        {
            // TODO (pafim37): This method should be more advanced
            longitude = longitude * 180; // convert to deg
            return (int)(7.5 + longitude) % 15;
        }

        private bool IsValidate(double param)
        {
            return param < Math.PI / 2 && param > - Math.PI / 2;
        }
    }
}
