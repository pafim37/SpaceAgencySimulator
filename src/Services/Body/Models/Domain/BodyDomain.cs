﻿using Sas.Body.Service.DataTransferObject;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain
{
    public class BodyDomain
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
        public BodyDomain(string name, double mass, Vector position, Vector velocity, double radius = 0)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mass, nameof(mass));
            ArgumentOutOfRangeException.ThrowIfNegative(mass, nameof(radius));

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
        public BodyDomain()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
        #endregion
    }
}
