using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain
{
    public class BodyBase
    {
        /// <summary>
        /// Name of the body. Should be unique
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Mass of the body
        /// </summary>
        public double Mass { get; }

        /// <summary>
        /// Position related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsolutePosition { get; }

        /// <summary>
        /// Velocity related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsoluteVelocity { get; }

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public Orbit Orbit { get; set; }

        /// <summary>
        /// Constructor of the Body
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="radius"></param>
        public BodyBase(string name, double mass, Vector position, Vector velocity, double radius)
        {
            Name = name;
            Mass = mass;
            AbsolutePosition = position;
            AbsoluteVelocity = velocity;
        }
    }
}
