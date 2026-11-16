using Sas.Notification.Service.Hubs;

namespace Sas.Notification.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSignalR();

            // CORS
            builder.Services.AddCors(o => o.AddPolicy("SasPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors("SasPolicy");
            app.UseAuthorization();

            app.MapHub<MasterHub>("/notification");

            app.Run();

        }
    }
}
