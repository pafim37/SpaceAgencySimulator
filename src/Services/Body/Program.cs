using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.Controllers;
using Sas.Body.Service.Repositories;

namespace Sas.Body.Service
{
    public class Program
    {
        public static void Main(string[] args)
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

            // CORS
            builder.Services.AddCors(o => o.AddPolicy("SasPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
            }));

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors("SasPolicy");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
