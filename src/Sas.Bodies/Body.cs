using Sas.Mathematica;

namespace Sas.Orbit.Bodies
{
    public class Body
    {
        public string Name { get; init; }
        public double Mass { get; init; }
        public Vector AbsolutePosition { get; set; }
        public Vector AbsoluteVelocity { get; set; }

        public Body( string name, double mass, Vector absolutePosition, Vector absoluteVelocity )
        {
            Name = name;
            Mass = mass;
            AbsolutePosition = absolutePosition;
            AbsoluteVelocity = absoluteVelocity;
        }

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
            return body.AbsoluteVelocity -AbsoluteVelocity;
        }

        public override string? ToString()
        {
            return $"Name: {Name}, Mass: {Mass}, AbsolutePosition: {AbsolutePosition}, AbsoluteVelocity: {AbsoluteVelocity}";
        }
    }
}
