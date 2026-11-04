using Microsoft.EntityFrameworkCore;
using Sas.Body.Infrastructure.Contexts;
using Sas.Body.Business.Handlers;
using Sas.Body.Business.Profiles;
using Sas.Body.Core.Notifications;
using Sas.Body.Infrastructure.Repositories;
using Sas.Body.Service.Filters;
using Sas.Body.Infrastructure.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IBodyRepository, BodyRepository>();

            builder.Services.AddMemoryCache();

            builder.Services.AddAutoMapper(_ => { }, typeof(BodyMapper).Assembly);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBodySystemHandler).Assembly));

            builder.Services.AddDbContext<BodyContext>(options =>
                options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]),
                ServiceLifetime.Scoped);

            builder.Services.AddControllers(options => options.Filters.Add<BodyExceptionFilter>())
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                 });

            builder.Services.AddSingleton(provider =>
            {
                return new NotificationClient();
            });
            builder.Services.AddSingleton<IBodyNotificationClient>(provider => provider.GetRequiredService<NotificationClient>());

            // CORS
            builder.Services.AddCors(o => o.AddPolicy("SasPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
            }));

            WebApplication app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                NotificationClient signalRClient = scope.ServiceProvider.GetRequiredService<NotificationClient>();
                signalRClient.StartConnection();
            }

            app.UseHttpsRedirection();

            app.UseCors("SasPolicy");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
