using Sas.Body.Service;
using Sas.Orbit.Service;
using Sas.BodySystem.Process.Handlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBodyService(builder.Configuration);
builder.Services.AddOrbitService();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateBodySystemHandler).Assembly));
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals);
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader()));
var app = builder.Build();
app.UseCors();
app.MapControllers();
app.Run();
