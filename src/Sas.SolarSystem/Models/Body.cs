using Sas.BodySystem.Orbits;
using Sas.Mathematica;

namespace Sas.BodySystem.Models
{
    public class Body
    {
        private readonly string _name;
        private double _mass;
        private readonly double _radius;
        private Vector _position;
        private Vector _velocity;

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

        /// <summary>
        /// Size of body
        /// </summary>
        public double Radius => _radius;

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public Orbit? Orbit { get; set; }

        /// <summary>
        /// Surrounding body
        /// </summary>
        public Body? SurroundedBody { get; set; }



        public Body(string name, double mass, Vector position, Vector velocity)
        {
            _name = name;
            _mass = mass;
            _position = position;
            _velocity = velocity;
        }

        public Body(string name, double mass, Vector position, Vector velocity, double radius)
        {
            _name = name;
            _mass = mass;
            _position = position;
            _velocity = velocity;
            _radius = radius;
        }

        public override string? ToString()
        {
            return $"Name: {Name}, Mass: {Mass}, AbsolutePosition: {AbsolutePosition}, AbsoluteVelocity: {AbsoluteVelocity}, SurroundedBody: <{SurroundedBody?.Name}>";
        }
    }
}
