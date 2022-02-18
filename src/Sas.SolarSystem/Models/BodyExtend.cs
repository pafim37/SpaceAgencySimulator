using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain.Bodies.old
{
    public class BodyExtend : BodyBase
    {
        

        public void GetOrbit()
        {
            var pos = this.GetPositionRelatedToSurroundedBody();
            var vel = this.GetVelocityRelatedToSurroundedBody();
            var u = (this.Mass + SurroundedBody.Mass) * Constants.G; 
            Orbit orbit = new Orbit(pos, vel, u);
            Orbit = orbit;
        }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(BodyBase body)
        {
            if (body != null)
            {
                return body.AbsolutePosition - this.AbsolutePosition;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedToSurroundedBody()
        {
            if (SurroundedBody != null)
            {
                return SurroundedBody.AbsolutePosition - this.AbsolutePosition;
            }
            else
                return Vector.Zero;
        }

        /// <summary>
        /// Returns velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedTo(BodyBase body)
        {
            if (body != null)
            {
                return body.AbsoluteVelocity - this.AbsoluteVelocity;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns velocity in relation to the surrounded body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedToSurroundedBody()
        {
            if (SurroundedBody != null)
            {
                return SurroundedBody.AbsoluteVelocity - this.AbsoluteVelocity;
            }
            else
                return Vector.Zero;
        }

        /// <summary>
        /// Returns sphere of influence
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public double GetSphereOfInfluence(BodyBase body)
        {
            if (body != null)
            {
                double distance = (AbsolutePosition - body.AbsolutePosition).Magnitude();
                double massRatio = Math.Pow(Mass / body.Mass, 0.4);
                return distance * massRatio;
            }
            else
                return 0.0;
        }

        public BodyExtend(string name, double mass, Vector position, Vector velocity) 
            : base(name, mass, position, velocity)
        {

        }
    }
}
