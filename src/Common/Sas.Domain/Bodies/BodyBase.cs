using Sas.Domain.Exceptions;
using Sas.Mathematica;

namespace Sas.Domain.Bodies
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
        public double Mass { get; set; }

        /// <summary>
        /// Position related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsolutePosition { get; set; }

        /// <summary>
        /// Velocity related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsoluteVelocity { get; set; }

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public Orbit Orbit { get; set; }

        /// <summary>
        /// Surrounding body
        /// </summary>
        public BodyBase SurroundedBody { get; set; }

        /// <summary>
        /// Returns position related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(BodyBase body)
        {
            if (body is not null)
            {
                return this.AbsolutePosition - body.AbsolutePosition;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns position related to surrounding body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedToSurroundedBody()
        {
            if (this.SurroundedBody is not null)
            {
                return this.AbsolutePosition - this.SurroundedBody.AbsolutePosition;
            }
            else
                throw new SurroundedBodyException("Surrounded body is not assigned");
        }

        /// <summary>
        /// Returns velocity related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedTo(BodyBase body)
        {
            if (body is not null)
            {
                return this.AbsoluteVelocity - body.AbsoluteVelocity;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns velocity related to surrounding body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedToSurroundedBody()
        {
            if (this.SurroundedBody is not null)
            {
                return SurroundedBody.AbsoluteVelocity - this.AbsoluteVelocity;
            }
            else
                throw new SurroundedBodyException("Surrounded body is not assigned");
        }

        /// <summary>
        /// Returns sphere of influence
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public double GetSphereOfInfluence(BodyBase body)
        {
            if (body is not null)
            {
                double distance = (this.AbsolutePosition - body.AbsolutePosition).Magnitude();
                double massRatio = Math.Pow(this.Mass / body.Mass, 0.4);
                return distance * massRatio;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }
        
        /// <summary>
        /// Update orbit of the body
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SurroundedBodyException"></exception>
        public void UpdateOrbit()
        {
            if (this.SurroundedBody is not null)
            {
                this.Orbit = new Orbit(this);
            }
            else
                throw new SurroundedBodyException("Surrounded body is not assigned");
        }

        /// <summary>
        /// Constructor of the Body
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        public BodyBase(string name, double mass, Vector position, Vector velocity)
        {
            Name = name;
            Mass = mass;
            AbsolutePosition = position;
            AbsoluteVelocity = velocity;
        }
    }
}
