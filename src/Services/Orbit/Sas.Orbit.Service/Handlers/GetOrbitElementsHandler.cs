using MediatR;
using Sas.Orbit.Service.Models.Requests;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Handlers
{
    public class GetOrbitElementsHandler : IRequestHandler<GetOrbitRequest, OrbitElements>
    {
        public Task<OrbitElements> Handle(GetOrbitRequest request, CancellationToken cancellationToken)
        {
            OrbitElements OrbitElements = OrbitElementsFactory.GetOrbitElements(request.Position, request.Velocity, request.GravitationalParameter);
            return Task.FromResult(OrbitElements);
        }
    }
}
