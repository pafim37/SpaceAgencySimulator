
// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Options;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.Data;
using Sas.BodySystem.Service.DAL;
using Sas.BodySystem.Service.Data;
using Sas.BodySystem.Service.Hubs;
using Sas.BodySystem.Service.Settings;

namespace Sas.Sandbox.Process
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // General Configuration
            builder.Configuration.AddJsonFile("config.json");

            // Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Astronomy Service
            builder.Services.AddScoped<AstronomyContext>();

            builder.Services.AddScoped<ObservatoryRepository>();
            builder.Services.AddScoped<ObservationRepository>();

            // Solar System Service
            builder.Services.Configure<BodyDatabaseSettings>(builder.Configuration.GetRequiredSection("DatabaseSettings"));
            builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<BodyDatabaseSettings>>().Value);
            builder.Services.AddSingleton<IBodyContext, BodyContext>();
            builder.Services.AddScoped<IBodyRepository, BodyRepository>();

            // SignalR
            builder.Services.AddSignalR();

            // CORS
            builder.Services.AddCors(options =>
            {
                //options.AddPolicy("AllowSpecificOrigin",
                //    builder => builder.WithOrigins("http://localhost:3000")
                //                      .AllowAnyHeader()
                //                      .AllowAnyMethod());
                //options.AddPolicy("AllowSignalR",
                //    builder => builder.WithOrigins("http://localhost:5000")
                //                      .AllowAnyHeader()
                //                      .AllowAnyMethod());
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

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

            app.MapHub<BodySystemHub>("/hub/body-system");

            app.UseCors(); // "AllowSpecificOrigin");

            app.Run();
        }
    }
}