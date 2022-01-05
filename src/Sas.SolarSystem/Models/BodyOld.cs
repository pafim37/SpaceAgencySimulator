using Sas.BodySystem.Orbits;
using Sas.Mathematica;

namespace Sas.BodySystem.Models
{
    public class BodyOld
    {
        private readonly string _name;
        private double _mass;
        private Vector _position;
        private Vector _velocity;
        private readonly double _radius;

        /// <summary>
        /// Name of the body. Should be unique
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Mass of the body
        /// </summary>
        public double Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }

        /// <summary>
        /// Position related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsolutePosition
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Velocity related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsoluteVelocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public double Radius => _radius;

        public BodyOld(string name, double mass, Vector position, Vector velocity)
        {
            _name = name;
            _mass = mass;
            _position = position;
            _velocity = velocity;
        }

        public BodyOld(string name, double mass, Vector position, Vector velocity, double radius)
        {
            _name = name;
            _mass = mass;
            _position = position;
            _velocity = velocity;
            _radius = radius;
        }

        /// <summary>
        /// G * (M + m)
        /// </summary>
        public double U
        {
            get
            {
                if (SurroundedBody != null)
                {
                    return (_mass + SurroundedBody.Mass) * Constants.G;
                }
                else
                    return 0.0;
            }
        }

        /// <summary>
        /// Surrounding body
        /// </summary>
        public BodyOld? SurroundedBody { get; set; }

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public Orbit? Orbit { get; set; }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(BodyOld? body)
        {
            if (body != null)
            {
                return body.AbsolutePosition - _position;
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
                return SurroundedBody.AbsolutePosition - _position;
            }
            else
                return Vector.Zero;
        }

        /// <summary>
        /// Velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedTo(BodyOld? body)
        {
            if (body != null)
            {
                return body.AbsoluteVelocity - _velocity;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedToSurroundedBody()
        {
            if (SurroundedBody != null)
            {
                return SurroundedBody.AbsoluteVelocity - _velocity;
            }
            else
                return Vector.Zero;
        }

        public double GetSphereOfInfluence(BodyOld body)
        {
            if (body != null)
            {
                double distance = (_position - body.AbsolutePosition).Magnitude();
                double massRatio = Math.Pow(Mass / body.Mass, 0.4);
                return distance * massRatio;
            }
            else
                return 0.0;
        }

        public override string? ToString()
        {
            return $"Name: {Name}, Mass: {Mass}, AbsolutePosition: {AbsolutePosition}, AbsoluteVelocity: {AbsoluteVelocity}, SurroundedBody: <{SurroundedBody?.Name}>";
        }
    }
}
