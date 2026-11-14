using MediatR;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Models.Requests
{
    public sealed record GetOrbitRequest(Vector Position, Vector Velocity, double GravitationalParameter) : IRequest<OrbitElements> { }
}
