using MediatR;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits;

namespace Sas.Orbit.Service.Models.Requests
{
    public record GetPositionedOrbitRequest(string OrbitName, Vector Position, Vector Velocity, Vector GravitationalSourcePosition, double GravitationalParameter) : IRequest<IPositionedOrbit>;
}
