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
            return other is not null ?
                body.AbsolutePosition - other.AbsolutePosition :
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns velocity related to given body 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>The relative velocity</returns>
        public static Vector GetVelocityRelatedTo(this Body body, Body other)
        {
            return other is not null ?
                body.AbsoluteVelocity - other.AbsoluteVelocity :
                throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Returns sphere of influence
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double GetSphereOfInfluenceRelatedTo(this Body body, Body other)
        {
            if (other is not null)
            {
                double distance = (body.AbsolutePosition - other.AbsolutePosition).Magnitude;
                double massRatio = Math.Pow(body.Mass / other.Mass, 0.4);
                return distance * massRatio;
            }
            else
                throw new ArgumentNullException(nameof(body));
        }
    }
}
