using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sas.Body.Business.Handlers;
using Sas.Body.Business.Profiles;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;
using Sas.Body.Infrastructure.Contexts;
using Sas.Body.Infrastructure.Notifications;
using Sas.Body.Infrastructure.Repositories;
using Sas.Body.Service.Controllers;
using Sas.Body.Service.Filters;

namespace Sas.Body.Service;

public static class BodyServiceCollectionExtensions
{
    /// <summary>Registers the Body module in an ASP.NET Core host.</summary>
    public static IServiceCollection AddBodyService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBodyRepository, BodyRepository>();
        services.AddAutoMapper(_ => { }, typeof(BodyMapper).Assembly);
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetBodiesHandler).Assembly));
        services.AddDbContext<BodyContext>(options => options.UseSqlServer(configuration["ConnectionStrings:Default"]));
        services.AddControllers(options => options.Filters.Add<BodyExceptionFilter>())
            .AddApplicationPart(typeof(BodyController).Assembly)
            .AddJsonOptions(options => options.JsonSerializerOptions.NumberHandling =
                System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals);
        services.AddSingleton<NotificationClient>();
        services.AddSingleton<IBodyNotificationClient>(provider => provider.GetRequiredService<NotificationClient>());
        services.AddHostedService<BodyNotificationHostedService>();
        return services;
    }
}
