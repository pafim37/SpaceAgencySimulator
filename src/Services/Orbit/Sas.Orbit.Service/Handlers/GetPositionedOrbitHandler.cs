using MediatR;
using Sas.Orbit.Service.Models.Requests;
using Sas.Orbit.Service.Orbits;

namespace Sas.Orbit.Service.Handlers
{
    public class GetPositionedOrbitHandler(IPositionedOrbitFactory orbitFactory) : IRequestHandler<GetPositionedOrbitRequest, IPositionedOrbit>
    {
        public Task<IPositionedOrbit> Handle(GetPositionedOrbitRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(orbitFactory.Create(request.OrbitName, request.Position, request.Velocity,
                request.GravitationalSourcePosition, request.GravitationalParameter));
        }
    }
}
