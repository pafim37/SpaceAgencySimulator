using Sas.Domain.Models.Bodies;
using Sas.Domain.Models.Orbits;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Bodies.BodyExtensions
{
    public static class BodyExtension
    {
        /// <summary>
        /// Returns position related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public static Vector GetPositionRelatedTo(this Body body, Body other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));
            return body.Position - other.Position;
        }

        /// <summary>
        /// Returns velocity related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public static Vector GetVelocityRelatedTo(this Body body, Body other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));
            return body.Velocity - other.Velocity;
        }

        /// <summary>
        /// Returns sphere of influence
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double GetSphereOfInfluenceRelatedTo(this Body body, Body other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));
            double distance = (body.Position - other.Position).Magnitude;
            double massRatio = Math.Pow(body.Mass / other.Mass, 0.4);
            return distance * massRatio;
        }
    }
}
