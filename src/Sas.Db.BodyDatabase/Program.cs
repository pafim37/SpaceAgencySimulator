// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sas.Db.BodyDatabase.Data;
using Sas.Db.BodyDatabase.Documents;
using Sas.Db.BodyDatabase.Repositories;
using Sas.Db.BodyDatabase.Settings;
using System.Configuration;

var host = CreateHostBuilder(args).Build();

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(app =>
        {
            app.AddJsonFile("config.json");
        })
        .ConfigureServices( (context, services) => {
            services.Configure<BodyDatabaseSettings>(context.Configuration.GetSection("DatabaseSettings"));
            services.AddScoped<IBodyDatabaseSettings, BodyDatabaseSettings>();
            services.AddSingleton<IBodyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<BodyDatabaseSettings>>().Value);
            services.AddScoped<IBodyDatabase, BodyDatabase>();
            services.AddSingleton<BodyRepository>();

        });
        
}

var rep1 = host.Services.GetRequiredService<BodyRepository>();
//var rep = host.Services.GetRequiredService<BodyDatabase>();

// BodyDocument bd = new BodyDocument() { Name = "Point", Mass = 1, Radius = 1, Position = new() { X = 1, Y = 1, Z = 1 }, Velocity = new() { X = 1, Y = 1, Z = 1 } };
// rep.Create(bd);

var o = await rep1.GetAsync("Point");
Console.WriteLine(o.Position.Y);
//foreach (var item in o)
//{
//    Console.WriteLine(item.Name);
//}
//var res = await rep.GetBodyByNameAsync("One");

//Console.WriteLine(res.Name);

