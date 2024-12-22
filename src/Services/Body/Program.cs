
using Sas.Body.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Repositories;
using Sas.Body.Service.Controllers;
using Sas.Body.Service.Handlers;

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

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
