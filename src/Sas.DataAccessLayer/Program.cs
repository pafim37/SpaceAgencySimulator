// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sas.BodyDatabase.Database;
using Sas.DataAccessLayer.Repositories;

var host = CreateHostBuilder(args).Build();

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(service =>
        {
            service.AddTransient<BodyRepository>();
            service.AddDbContext<BodyContext>();
        });
}

var rep = host.Services.GetRequiredService<BodyRepository>();
var res = await rep.GetBodyByNameAsync("One");

Console.WriteLine(res.Name);
