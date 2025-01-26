using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Extensions.BodyExtensions
{
    public static class BodyDomainExtension
    {
        /// <summary>
        /// Returns position related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative position</returns>
        public static Vector GetPositionRelatedTo(this BodyDomain body, BodyDomain other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));
            return body.Position - other.Position;
        }

        /// <summary>
        /// Returns velocity related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public static Vector GetVelocityRelatedTo(this BodyDomain body, BodyDomain other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));
            return body.Velocity - other.Velocity;
        }

        /// <summary>
        /// Returns sphere of influence
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double GetSphereOfInfluenceRelatedTo(this BodyDomain body, BodyDomain other)
        {
            ArgumentNullException.ThrowIfNull(other, nameof(other));
            double distance = (body.Position - other.Position).Magnitude;
            double massRatio = Math.Pow(body.Mass / other.Mass, 0.4);
            return distance * massRatio;
        }
    }
}
