using Sas.Domain.Exceptions;
using Sas.Domain.Orbits;
using Sas.Mathematica;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Bodies
{
    public class BodyBase : PointParticle
    {
        #region properties

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public OrbitBase Orbit { get; set; }

        /// <summary>
        /// Surrounding body
        /// </summary>
        public PointParticle SurroundedBody { get; set; }

        #endregion

        #region methods

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
        /// Update orbit of the body
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SurroundedBodyException"></exception>
        public void UpdateOrbit()
        {
            if (this.SurroundedBody is not null)
            {
                this.Orbit = new OrbitBase(this);
            }
            else
                throw new SurroundedBodyException("Surrounded body is not assigned");
        }

        #endregion

        #region constructors

        /// <summary>
        /// Constructor of the BodyBase
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        public BodyBase(string name, double mass, Vector position, Vector velocity)
            : base(name, mass, position, velocity)
        {
            Name = name;
            Mass = mass;
            AbsolutePosition = position;
            AbsoluteVelocity = velocity;
        }

        /// <summary>
        /// Parameterless constructor - need for mapping
        /// </summary>
        public BodyBase()
            : base()
        {
        }

        #endregion
    }
}
