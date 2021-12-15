using Sas.Mathematica;

namespace Sas.Sandbox.Models
{
    public class Body
    {
        public string Name { get; init; }
        public double Mass { get; init; }
        public Vector AbsolutePosition { get; set; }
        public Vector AbsoluteVelocity { get; set; }
        public Body? Attracted { get; set; }

        /// <summary>
        /// Position in relation to the body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public Vector GetPositionRelatedTo(Body body)
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
            return body.AbsoluteVelocity - AbsoluteVelocity;
        }

        public double GetSphereOfInfluence(double phi)
        {
            CoordinateSystem cs = new();
            cs.Cartesian(AbsoluteVelocity);
            double distance = (AbsolutePosition - Attracted.AbsolutePosition).Magnitude();
            double massRatio = Math.Pow(Mass / Attracted.Mass, 0.4);
            return distance * massRatio / Math.Pow(1 + 3 * Math.Pow(Math.Cos(phi - cs.Phi), 2), 0.1);
        }

        public override string? ToString()
        {
            return $"Name: {Name}, Mass: {Mass}, AbsolutePosition: {AbsolutePosition}, AbsoluteVelocity: {AbsoluteVelocity}";
        }

        public class Builder
        {
            private string _name;
            private double _mass;
            private Vector _absolutePosition;
            private Vector _absoluteVelocity;

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

            public Body Build()
            {
                return new Body { Name = _name, Mass = _mass, AbsolutePosition = _absolutePosition, AbsoluteVelocity = _absoluteVelocity };
            }

        }
    }
}
