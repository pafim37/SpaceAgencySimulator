using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.Controllers;
using Sas.Body.Service.Notifications;
using Sas.Body.Service.Repositories;

namespace Sas.Body.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IBodyRepository, BodyRepository>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BodySystemController).Assembly));

            builder.Services.AddDbContext<BodyContext>(options =>
                options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]),
                ServiceLifetime.Scoped);

            builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                 });

            builder.Services.AddSingleton(provider =>
            {
                var hubUrl = "http://localhost:6443/notification";
                return new NotificationService(hubUrl);
            });

            // CORS
            builder.Services.AddCors(o => o.AddPolicy("SasPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
            }));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var signalRClient = scope.ServiceProvider.GetRequiredService<NotificationService>();
                await signalRClient.StartConnectionAsync();
            }

            app.UseHttpsRedirection();

            app.UseCors("SasPolicy");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
