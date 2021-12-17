using Sas.Mathematica;
using Sas.SolarSystem.Orbits;

namespace Sas.SolarSystem.Models
{
    public class Body
    {
        /// <summary>
        /// Name of the body. Should be unique
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// Mass of the body
        /// </summary>
        public double Mass { get; private set; }

        /// <summary>
        /// Body type: star, earth, moon etc...
        /// </summary>
        public BodyType? BodyType { get; private set; }

        /// <summary>
        /// Position related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector? AbsolutePosition { get; set; }

        /// <summary>
        /// Velocity related to center of the solar system. Should be used for drawing on board
        /// </summary>
        public Vector? AbsoluteVelocity { get; set; }

        /// <summary>
        /// G * (M + m)
        /// </summary>
        public double U
        {
            get
            {
                if (this.SurroundedBody == null) throw new Exception("Cannot get U because there is no SurroundedBody body");
                return Constants.G * (Mass + this.SurroundedBody.Mass);
            }
        }


        /// <summary>
        /// Surrounding body
        /// </summary>
        public Body? SurroundedBody { get; set; }

        /// <summary>
        /// Orbit of the body
        /// </summary>
        public Orbit? Orbit { get; set; }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(Body? body)
        {
            if (body?.AbsolutePosition == null || AbsolutePosition == null) throw new ArgumentException();
            else
            {
                return body.AbsolutePosition - this.AbsolutePosition;
            }
        }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedToSurroundedBody()
        {
            if (SurroundedBody?.AbsolutePosition == null || this.AbsolutePosition == null)
            {
                return new Vector(0, 0, 0);
            }
            else
            {
                return SurroundedBody.AbsolutePosition - AbsolutePosition;
            }
        }

        /// <summary>
        /// Velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedTo(Body? body)
        {
            if (body?.AbsoluteVelocity == null || this.AbsoluteVelocity == null) throw new ArgumentException();
            return body.AbsoluteVelocity - this.AbsoluteVelocity;
        }

        /// <summary>
        /// Velocity in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public Vector GetVelocityRelatedToSurroundedBody()
        {
            if(SurroundedBody?.AbsoluteVelocity == null || this.AbsoluteVelocity == null) throw new Exception($"Surrounded body for {Name} doesn't exist");
            return SurroundedBody.AbsoluteVelocity - this.AbsoluteVelocity;
        }

        public double GetSphereOfInfluence(Body body)
        {
            if(AbsoluteVelocity == null || AbsolutePosition == null || body.AbsolutePosition == null) throw new Exception();
            CoordinateSystem cs = new();
            cs.Cartesian(AbsoluteVelocity);
            double distance = (AbsolutePosition - body.AbsolutePosition).Magnitude();
            double massRatio = Math.Pow(Mass / body.Mass, 0.4);
            return distance * massRatio;
        }

        public override string? ToString()
        {
            return $"Name: {Name}, Type: {BodyType}, Mass: {Mass}, SurroundedBody: {SurroundedBody.Name}, AbsolutePosition: {AbsolutePosition}, AbsoluteVelocity: {AbsoluteVelocity}, ";
        }

        public class Builder
        {
            private string? _name;
            private double _mass;
            private Body? _surrounded;
            private Vector? _absolutePosition;
            private Vector? _absoluteVelocity;
            private BodyType _type;

            public Builder Name(string value)
            {
                _name = value;
                return this;
            }
            public Builder Mass(double value)
            {
                _mass = value;
                return this;
            }
            public Builder AbsolutePosition(Vector value)
            {
                _absolutePosition = value;
                return this;
            }
            public Builder AbsoluteVelocity(Vector value)
            {
                _absoluteVelocity = value;
                return this;
            }

            public Builder Type(BodyType value)
            {
                _type = value;
                return this;
            }

            public Builder SurroundedBody(Body value)
            {
                _surrounded = value;
                return this;
            }

            public Body Build()
            {
                if (_absolutePosition == null) throw new ArgumentNullException("No absolute position");
                if (_absoluteVelocity == null) throw new ArgumentNullException("No absolute velocity");
                return new Body { Name = _name, Mass = _mass, AbsolutePosition = _absolutePosition, AbsoluteVelocity = _absoluteVelocity, BodyType = _type, SurroundedBody = _surrounded };
            }

        }
    }
}
