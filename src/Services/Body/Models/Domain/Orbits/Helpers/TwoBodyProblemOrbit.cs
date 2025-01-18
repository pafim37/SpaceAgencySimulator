using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Models.Domain.BodyExtensions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Domain.Exceptions;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Helpers
{
    public class TwoBodyProblemOrbit
    {
        private const double TwoBodyProblemMassRatioLimit = 0.03;

        public static Orbit? Calculate(BodyDomain body, BodyDomain other, double G)
        {
            if (body.Mass / other.Mass > TwoBodyProblemMassRatioLimit)
            {
                throw new TwoBodyProblemAssumptionNotSatisfiedException();
            }

            Vector position = body.GetPositionRelatedTo(other);
            Vector velocity = body.GetVelocityRelatedTo(other);
            double u = G * (body.Mass + other.Mass);
            Orbit orbit;
            try
            {
                orbit = OrbitFactory.CalculateOrbit(position, velocity, u);
            }
            catch (UnknownOrbitTypeException)
            {
                throw;
            }
            orbit.Name = body.Name;
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                double rotationAngle = orbit.RotationAngle;
                orbit.Center = new Vector(other.Position.X - Math.Cos(rotationAngle) * orbit.Eccentricity * orbit.SemiMajorAxis!.Value, Math.Sin(rotationAngle) * orbit.Eccentricity * orbit.SemiMajorAxis.Value + other.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Hyperbolic)
            {
                double rotationAngle = -orbit.RotationAngle;
                orbit.Center = new Vector(orbit.MinDistance - orbit.SemiMajorAxis!.Value + Math.Cos(rotationAngle) * other.Position.X, orbit.MinDistance - orbit.SemiMajorAxis.Value + other.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Circular)
            {
                orbit.Center = new Vector(other.Position.X, other.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Parabolic)
            {
                orbit.Center = new Vector(other.Position.Y, other.Position.X, 0);
            }
            return orbit;
        }
    }
}
