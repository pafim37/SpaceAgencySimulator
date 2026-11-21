using Sas.Body.Infrastructure.Contexts;
using Sas.Body.Service;
using Sas.BodySystem.Process.Handlers;
using Sas.BodySystem.Process.Models.Domain.Orbits;
using Sas.Orbit.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddBodyService(builder.Configuration);
builder.Services.AddSingleton<IPositionedOrbitFactory, PositionedOrbitFactory>();

builder.Services.AddOrbitService();
builder.Services.AddMemoryCache();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateOrUpdateBodySystemHandler).Assembly));
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals);
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader()));
WebApplication app = builder.Build();

if (app.Configuration.GetValue<bool>("Body:EnsureDatabaseCreated"))
{
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
    BodyContext bodyContext = scope.ServiceProvider.GetRequiredService<BodyContext>();
    await bodyContext.Database.EnsureCreatedAsync();
}

app.UseCors();
app.MapControllers();
app.Run();
