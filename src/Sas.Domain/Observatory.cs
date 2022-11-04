using Sas.Domain.Observations;

namespace Sas.Domain
{
    public class Observatory
    {
        /// <summary>
        /// Name of the observatory
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Latitude of the observatory expressed in radians
        /// </summary>
        public double LatitudeRad { get; }

        /// <summary>
        /// Longitude of the observatory expressed in radians
        /// </summary>
        public double LongitudeRad { get; }

        /// <summary>
        /// Height of the observatory above mean sea level
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Constructor of the observatory class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public Observatory(string name, double latitude, double longitude, double height)
        {
            // TODO (pafim37): improve Validation
            if (IsValidate(latitude) && IsValidate(longitude) && height >= 0)
            {
                Name = name;
                LatitudeRad = latitude;
                LongitudeRad = longitude;
                Height = height;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Latitude or Longitude out of range exception");
            }
        }

        #region private methods
        private bool IsValidate(double param)
        {
            return param < Math.PI && param > -Math.PI;
        }
        #endregion
    }
}
