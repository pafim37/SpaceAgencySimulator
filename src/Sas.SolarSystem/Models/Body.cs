using Sas.Mathematica;
using Sas.SolarSystem.Orbits;

namespace Sas.SolarSystem.Models
{
    public class Body
    {
        private string _name;
        private double _mass;
        private Vector _position;
        private Vector _velocity;

        /// <summary>
        /// Name of the body. Should be unique
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Mass of the body
        /// </summary>
        public double Mass => _mass;

        /// <summary>
        /// Position related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsolutePosition => _position;

        /// <summary>
        /// Velocity related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector AbsoluteVelocity => _velocity;

        public Body(string name, double mass, Vector position, Vector velocity)
        {
            _name = name;
            _mass = mass;
            _position = position;
            _velocity = velocity;
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
                {
                    throw new ArgumentNullException($"Surrounded body for {Name} doesn't exist");
                }
            }
        }

        /// <summary>
        /// Surrounding body
        /// </summary>
        public Body SurroundedBody { get; set; }

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public Orbit Orbit { get; set; }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(Body? body)
        {
            if (body != null)
            {
                return body.AbsolutePosition - _position;
            }
            else
                throw new ArgumentNullException();
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
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedTo(Body? body)
        {
            if (body != null)
            {
                return body.AbsoluteVelocity - _velocity;
            }
            else
                throw new ArgumentNullException();
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
                throw new ArgumentNullException($"Surrounded body for {Name} doesn't exist");
        }

        public double GetSphereOfInfluence(Body body)
        {
            if (body != null)
            {
                double distance = (_position - body.AbsolutePosition).Magnitude();
                double massRatio = Math.Pow(Mass / body.Mass, 0.4);
                return distance * massRatio;
            }
            else
                throw new ArgumentNullException();
        }

        public override string? ToString()
        {
            return $"Name: {Name}, Mass: {Mass}, AbsolutePosition: {AbsolutePosition}, AbsoluteVelocity: {AbsoluteVelocity}, SurroundedBody: {SurroundedBody.Name}";
        }
    }
}
