﻿using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Extensions.BodyExtensions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.OrbitInfos;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Domain.Exceptions;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Helpers
{
    public class TwoBodyProblemOrbit
    {
        private const double TwoBodyProblemMassRatioLimit = 0.03;

        public static PositionedOrbit Calculate(BodyDomain body, BodyDomain other, double G)
        {
            if (body.Mass / other.Mass > TwoBodyProblemMassRatioLimit)
            {
                throw new TwoBodyProblemAssumptionNotSatisfiedException();
            }

            Vector position = body.GetPositionRelatedTo(other);
            Vector velocity = body.GetVelocityRelatedTo(other);
            double u = G * (body.Mass + other.Mass);

            OrbitDescription orbitDescription;
            try
            {
                orbitDescription = OrbitInfoDescription.CalculateOrbit(position, velocity, u);
            }
            catch (UnknownOrbitTypeException)
            {
                throw;
            }

            PositionedOrbit positionedOrbit = new() { OrbitDescription = orbitDescription };
            orbitDescription.Name = body.Name;
            positionedOrbit.Center = GetCenter(other, orbitDescription, positionedOrbit);
            return positionedOrbit;
        }

        private static Vector GetCenter(BodyDomain other, OrbitDescription orbitDescription, PositionedOrbit positionedOrbit)
        {
            switch (orbitDescription.OrbitType)
            {
                case OrbitType.Elliptic:
                    {
                        if (orbitDescription.SemiMajorAxis == null)
                        {
                            throw new InvalidOperationException("SemiMajorAxis cannot be null.");
                        }
                        double ae = orbitDescription.Eccentricity * orbitDescription.SemiMajorAxis.Value;
                        ReferenceSystem rs = new(other.Position, orbitDescription.EccentricityVector.Normalize());
                        return -ae * rs.GetNormalizedVector();
                    }

                case OrbitType.Hyperbolic:
                    {
                        if (orbitDescription.SemiMajorAxis == null)
                        {
                            throw new InvalidOperationException("SemiMajorAxis cannot be null.");
                        }
                        double ae = orbitDescription.Eccentricity * orbitDescription.SemiMajorAxis.Value;
                        ReferenceSystem rs = new(other.Position, orbitDescription.EccentricityVector.Normalize());
                        return -ae * rs.GetNormalizedVector();
                    }

                case OrbitType.Circular:
                    return new Vector(other.Position.X, other.Position.Y, 0);
                case OrbitType.Parabolic:
                    return new Vector(other.Position.Y, other.Position.X, 0);
                default:
                    throw new UnknownOrbitTypeException("Unknown orbit type while determine a center of the orbit.");
            }
        }
    }
}
