using Microsoft.Extensions.DependencyInjection;
using Sas.Orbit.Service.Controllers;
using Sas.Orbit.Service.Handlers;

namespace Sas.Orbit.Service;

public static class OrbitServiceCollectionExtensions
{
    public static IServiceCollection AddOrbitService(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetPositionedOrbitHandler).Assembly));
        services.AddControllers().AddApplicationPart(typeof(OrbitController).Assembly);
        return services;
    }
}
