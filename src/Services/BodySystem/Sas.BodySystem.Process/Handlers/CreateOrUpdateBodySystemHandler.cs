using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.BodySystem.Process.Models.Domain.Bodies;
using Sas.BodySystem.Process.Models.Domain.Orbits;
using Sas.BodySystem.Process.Models.Queries;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.BodySystem.Process.Handlers;


public sealed class CreateOrUpdateBodySystemHandler(IMediator mediator, IOrbitFactory orbitFactory, IMemoryCache memoryCache) : IRequestHandler<CreateBodySystemRequest, BodySystemDto>
{
    private const string BodySytemCacheKey = "BodySystem";
    public async Task<BodySystemDto> Handle(CreateBodySystemRequest request, CancellationToken cancellationToken)
    {
        if (!memoryCache.TryGetValue(BodySytemCacheKey, out BodySystems.BodySystem? bodySystem))
        {
            IEnumerable<BodyDomain> enabledBodies = await GetEnabledBodies(cancellationToken);
            BodySystems.BodySystem system = new(enabledBodies, orbitFactory, request.GravitationalConstant);
            system.UpdateBodySystem(request.Scale, request.AtTime, cancellationToken);
            memoryCache.Set(BodySytemCacheKey, system);
            return MapToBodySystemDto(system);
        }
        else
        {
            bodySystem!.FindAllOrbits(cancellationToken);
            return MapToBodySystemDto(bodySystem);
        }
    }

    private static BodySystemDto MapToBodySystemDto(BodySystems.BodySystem bodySystem)
    {
        return new BodySystemDto()
        {
            GravitationalConstant = bodySystem.GravitationalConstant,
            Barycentrum = bodySystem.Barycentrum,
            Bodies = bodySystem.Bodies,
            Orbits = [.. bodySystem.Orbits.Select(ToOrbitData)]
        };
    }

    private static OrbitDto ToOrbitData(IOrbit orbit)
    {
        IOrbitElements elements = orbit.OrbitElements;
        return new OrbitDto
        {
            Name = orbit.Name,
            OrbitElements = new OrbitElementsDto
            {
                OrbitType = elements.OrbitType,
                SemiMajorAxis = elements.SemiMajorAxis,
                SemiMinorAxis = elements.SemiMinorAxis,
                Eccentricity = elements.Eccentricity,
                ArgumentOfPeriapsis = elements.ArgumentOfPeriapsis,
                Inclination = elements.Inclination,
                AscendingNode = elements.AscendingNode,
                TrueAnomaly = elements.TrueAnomaly,
                EccentricAnomaly = elements.EccentricAnomaly,
                MeanAnomaly = elements.MeanAnomaly,
                Period = elements.Period,
                Radius = elements.Radius,
                GravitationalParameter = elements.GravitationalParameter,
                FirstNode = elements.FirstNode,
                MeanMotion = elements.MeanMotion
            },
            Points = [.. orbit.Points.Select(point => new OrbitPointDto(point.X, point.Y, point.Z))]
        };
    }

    private async Task<IEnumerable<BodyDomain>> GetEnabledBodies(CancellationToken cancellationToken)
    {
        IEnumerable<BodyDto> bodies = await mediator.Send(new GetEnabledBodiesRequest(), cancellationToken);
        IEnumerable<BodyDomain> enabledBodies = bodies.Where(b => b?.Enabled == true).Select(b => new BodyDomain
        {
            Name = b?.Name ?? throw new InvalidOperationException("Body name is missing."),
            Mass = b?.Mass ?? throw new InvalidOperationException("Body mass is missing."),
            Position = b?.Position is { } p ? new Vector(p.X, p.Y, p.Z) : throw new InvalidOperationException("Body position is missing."),
            Velocity = b?.Velocity is { } v ? new Vector(v.X, v.Y, v.Z) : throw new InvalidOperationException("Body velocity is missing."),
            Radius = b?.Radius ?? 0
        });
        return enabledBodies;
    }
}
