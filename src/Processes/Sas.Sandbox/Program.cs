
// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sas.Dal.BodyDataAccessLayer.Repositories;
using Sas.Db.BodyDatabase.Data;
using Sas.Db.BodyDatabase.Settings;
using Sas.Sandbox;
using Sas.Service.Astronomy.Controllers;
using Sas.Service.Astronomy.DAL;
using Sas.Service.Astronomy.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<Context>();
builder.Services.AddScoped<ObservatoryRepository>();
builder.Services.AddScoped<ObservationRepository>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapGet("/", () => "Hello World");

app.Run();

////var host = CreateHostBuilder(args).Build();

////IHostBuilder CreateHostBuilder(string[] args)
////{
////    return Host.CreateDefaultBuilder(args)
////        .ConfigureServices(service =>
////        {
////            service.AddTransient<IRepository<Body>, Repository>();
////            // service.AddDbContext<BodyContext>();
////        });
////}

////var rep = host.Services.GetRequiredService<IRepository<Body>>();
////var res = await rep.GetByNameAsync("One");

////Console.WriteLine(res.Id);

////SolarSystem solarSystem = new SolarSystem();
////solarSystem.Init();

////foreach (var body in solarSystem.GetBodies())
////{
////    if (body.Name.Equals("Earth"))
////    {
////        Console.WriteLine($"{body.Name}, {body.U}, {body.Orbit.SemiLatusRectum}");
////    }
////}

////Body body1 = new Body("1", 1, new Vector(1, 0, 0), Vector.Zero);
////Body body2 = new Body("2", 1, new Vector(0, 1, 0), Vector.Zero);
////Body body3 = new Body("3", 1, new Vector(0, 0, 1), Vector.Zero);

////SolarSystem mySolarSystem = new SolarSystem();
////mySolarSystem.AddBody(body1);
////mySolarSystem.AddBody(body2);
////mySolarSystem.AddBody(body3);

////var bc = mySolarSystem.GetBarycentrum();
////Console.WriteLine(bc);

////foreach (var item in mySolarSystem.GetBodies())
////{
////    Console.WriteLine(item.Name);
////}

//var host = CreateHostBuilder(args).Build();

//IHostBuilder CreateHostBuilder(string[] args)
//{
//    return Host.CreateDefaultBuilder(args)
//        .ConfigureServices(service =>
//        {
//            //service.AddTransient<BodyRepository>();
//            service.AddDbContext<BodyContext>();
//        });
//}

////BodyRepository rep = host.Services.GetRequiredService<BodyRepository>();
//var b1 = await rep.GetBodyByNameAsync("body1");
//var b2 = await rep.GetBodyByNameAsync("body2");

//TwoBodySystem twoBodySystem = new(b1, b2);

//Console.WriteLine(b1);
//Console.WriteLine(b2);

//Console.WriteLine(twoBodySystem.U);
//Console.WriteLine(twoBodySystem.GetBarycenter());
//twoBodySystem.CalibrateBarycenterForZero();
//Console.WriteLine(twoBodySystem.GetBarycenter());
//Console.WriteLine(b1);
//Console.WriteLine(b2);


// var host = CreateHostBuilder(args).Build();



//static IHostBuilder CreateHostBuilder(string[] args)
//{
//    return Host.CreateDefaultBuilder(args)
//        //.ConfigureWebHostDefaults(webBuilder =>
//        //{
//        //    webBuilder.UseStartup<Startup>();
//        //})
//        .ConfigureAppConfiguration(app =>
//        {
//            app.AddJsonFile("config.json");
//        });
//        //.ConfigureServices((context, services) => {
//        //    services.Configure<BodyDatabaseSettings>(context.Configuration.GetSection("DatabaseSettings"));
//        //    services.AddScoped<IBodyDatabaseSettings, BodyDatabaseSettings>();
//        //    services.AddSingleton<IBodyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<BodyDatabaseSettings>>().Value);
//        //    services.AddScoped<IBodyDatabase, BodyDatabase>();
//        //    services.AddSingleton<BodyRepository>();

//        //    services.AddSingleton<ObservatoryRepository>();

//        //});

//}
//host.Run();

//var rep = host.Services.GetRequiredService<BodyDatabase>();

// BodyDocument bd = new BodyDocument() { Name = "Point", Mass = 1, Radius = 1, Position = new() { X = 1, Y = 1, Z = 1 }, Velocity = new() { X = 1, Y = 1, Z = 1 } };
// rep.Create(bd);

//var rep1 = host.Services.GetRequiredService<BodyRepository>();
//var o = await rep1.GetAsync();
//// Console.WriteLine(o.AbsolutePosition.Y);

//foreach (var item in o)
//{
//    Console.WriteLine(item.Name);
//}
//var res = await rep.GetBodyByNameAsync("One");

//Console.WriteLine(res.Name);

//Matrix matrix = new Matrix(new double[9] {1, 1, 1, 1, 1, 1, 1, 1, 1 });
//Vector vector = new Vector(2, 3, 1);
//Console.WriteLine(matrix * vector);
