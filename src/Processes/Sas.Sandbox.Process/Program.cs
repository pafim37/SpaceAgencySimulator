
// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Options;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.Data;
using Sas.SolarSystem.Service.DAL;
using Sas.SolarSystem.Service.Data;
using Sas.SolarSystem.Service.Settings;

namespace Sas.Sandbox.Process
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // General Configuration
            builder.Configuration.AddJsonFile("config.json");

            // Astronomy Service
            builder.Services.AddScoped<AstronomyContext>();

            builder.Services.AddScoped<ObservatoryRepository>();
            builder.Services.AddScoped<ObservationRepository>();

            // Solar System Service
            builder.Services.Configure<SolarSystemDatabaseSettings>(builder.Configuration.GetRequiredSection("DatabaseSettings"));
            builder.Services.AddSingleton<SolarSystemDatabaseSettings>(x => x.GetRequiredService<IOptions<SolarSystemDatabaseSettings>>().Value);
            builder.Services.AddSingleton<ISolarSystemContext, SolarSystemContext>();
            builder.Services.AddScoped<ICelestialBodyRepository, CelestialBodyRepository>();

            // Controllers
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );


            /// <summary>
            /// Add auto mapper dependancy injection
            /// </summary>
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            WebApplication app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}