using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Process.Models.Domain.Bodies
{
    public class BodyDomain
    {
        public BodyDomain()
        {
            // Default constructor for serialization
        }

        /// <summary>
        /// Name of the body. Should be unique
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Mass of the body
        /// </summary>
        public required double Mass { get; set; }

        /// <summary>
        /// Position related to center of the reference frame (e.g. center of the solar system)
        /// </summary>
        public required Vector Position { get; set; }

        /// <summary>
        /// Velocity related to center of the reference frame (e.g. center of the solar system)
        /// </summary>
        public required Vector Velocity { get; set; }

        /// <summary>
        /// Radius of the body
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Name of the revolving body
        /// </summary>
        public string? ParentName { get; set; }

        /// <summary>
        /// Radius of the sphere of influence
        /// </summary>
        public double? SphereOfInfluenceRadius { get; set; }
    }
}
