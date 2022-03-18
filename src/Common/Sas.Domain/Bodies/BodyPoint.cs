using Sas.Mathematica;

namespace Sas.Domain.Bodies
{
    public class BodyPoint
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
        /// Position related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsolutePosition { get; set; }

        /// <summary>
        /// Velocity related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsoluteVelocity { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Returns position related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(BodyPoint body)
        {
            if (body is not null)
            {
                return this.AbsolutePosition - body.AbsolutePosition;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns velocity related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedTo(BodyPoint body)
        {
            if (body is not null)
            {
                return this.AbsoluteVelocity - body.AbsoluteVelocity;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns sphere of influence
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public double GetSphereOfInfluence(BodyPoint body)
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

        #endregion

        #region constructors

        /// <summary>
        /// Constructor of the BodyPoint
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        public BodyPoint(string name, double mass, Vector position, Vector velocity)
        {
            Name = name;
            Mass = mass;
            AbsolutePosition = position;
            AbsoluteVelocity = velocity;
        }
        public BodyPoint()
        {
        }

        #endregion
    }
}
