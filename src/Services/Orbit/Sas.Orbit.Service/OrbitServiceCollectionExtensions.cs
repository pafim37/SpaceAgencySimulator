using Microsoft.Extensions.DependencyInjection;
using Sas.Orbit.Service.Controllers;
using Sas.Orbit.Service.Handlers;
using Sas.Orbit.Service.Orbits;

namespace Sas.Orbit.Service;

public static class OrbitServiceCollectionExtensions
{
    public static IServiceCollection AddOrbitService(this IServiceCollection services)
    {
        services.AddSingleton<IPositionedOrbitFactory, PositionedOrbitFactory>();
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetPositionedOrbitHandler).Assembly));
        services.AddControllers().AddApplicationPart(typeof(OrbitController).Assembly);
        return services;
    }
}
