using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Bodies
{
    public class Body
    {
        #region properties
        /// <summary>
        /// Name of the body. Should be unique
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mass of the body
        /// </summary>
        public double Mass { get; set; }

        /// <summary>
        /// Position related to center of the solar system
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Velocity related to center of the solar system
        /// </summary>
        public Vector Velocity { get; set; }

        /// <summary>
        /// Radius of the body
        /// </summary>
        public double Radius { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Constructor of the BodyPoint
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="radius"></param>
        public Body(string name, double mass, Vector position, Vector velocity, double radius = 0)
        {
            IsNotNegative(mass);
            IsNotNegative(radius);

            Name = name;
            Mass = mass;
            Position = position;
            Velocity = velocity;
            Radius = radius;
        }

        /// <summary>
        /// Parameterless constructor - needed for mapping
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Body()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
        #endregion

        #region private methods
        private void IsNotNegative(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"Negative number name of {nameof(value)}");
            }
        }
        #endregion
    }
}
