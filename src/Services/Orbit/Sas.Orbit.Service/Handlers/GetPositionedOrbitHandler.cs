using MediatR;
using Sas.Orbit.Service.Models.Requests;
using Sas.Orbit.Service.Orbits;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Handlers
{
    public class GetPositionedOrbitHandler : IRequestHandler<GetPositionedOrbitRequest, PositionedOrbit>
    {
        public Task<PositionedOrbit> Handle(GetPositionedOrbitRequest request, CancellationToken cancellationToken)
        {
            OrbitElements orbitElements = OrbitElementsFactory.GetOrbitElements(request.Position, request.Velocity, request.GravitationalParameter);
            return Task.FromResult(new PositionedOrbit(request.OrbitName, request.GravitationalSourcePosition, orbitElements));
        }
    }
}
