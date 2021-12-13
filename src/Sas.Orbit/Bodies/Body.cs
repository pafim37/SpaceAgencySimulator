using Sas.Mathematica.Models;
using Sas.Orbit.Frames;
using Sas.Orbit.Orbits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Orbit.Bodies
{
    public class Body
    {
        public double Mass { get; init; }
        public Vector? AbsolutePosition { get; set; }
        public Vector? AbsoluteVelocity { get; set; }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPosition(Body body)
        {
            if (body == null || AbsolutePosition == null)
            {
                return new Vector(0, 0, 0);
            }
            else
            {
                return body.AbsolutePosition - AbsolutePosition;
            }
            
        }

        /// <summary>
        /// Velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocity(Body body)
        {
            return body.AbsoluteVelocity -AbsoluteVelocity;
        }
    }
}
