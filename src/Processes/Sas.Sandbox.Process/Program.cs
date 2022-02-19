
// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.Data;
using Sas.Domain;
using Sas.Domain.Bodies;
using Sas.Mathematica;
using Sas.SolarSystem.Service.DAL;
using Sas.SolarSystem.Service.Data;
using Sas.SolarSystem.Service.Settings;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<AstronomyContext>();
builder.Services.AddScoped<ObservatoryRepository>();
builder.Services.AddScoped<ObservationRepository>();

builder.Configuration.AddJsonFile("config.json");
builder.Services.Configure<BodyDatabaseSettings>(builder.Configuration.GetRequiredSection("DatabaseSettings"));


builder.Services.AddSingleton<BodyDatabaseSettings>(x => x.GetRequiredService<IOptions<BodyDatabaseSettings>>().Value);

builder.Services.AddSingleton<ISolarSystemContext, SolarSystemContext>();
builder.Services.AddScoped<IBodyRepository, BodyRepository>();

builder.Services.AddControllers();

/// <summary>
/// Add auto mapper dependancy injection
/// </summary>
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapGet("/", () => Endpoints());


string Endpoints()
{
    string e = "Hello World" + '\n';
    e += "https://localhost:5001/" + "observatories" + '\n';
    e += "https://localhost:5001/" + "observatories/{name}" + '\n';
    e += "https://localhost:5001/" + "observatories/{id}" + '\n';
    e += "https://localhost:5001/" + "observations" + '\n';
    e += "https://localhost:5001/" + "observations/{name}" + '\n';
    e += "https://localhost:5001/" + "observations/extend/{id}" + '\n';
    e += "https://localhost:5001/" + "observations/extend/{id}" + '\n';
    e += "https://localhost:5001/" + "bodies" + '\n';
    return e;
}


BodyBase Sun = new("Sun", Constants.SolarMass, Vector.Zero, Vector.Zero);

BodyBase Earth = new(
    "Earth",
    Constants.EarthMass,
    new Vector(Constants.EarthApoapsis, 0, 0),
    new Vector(0, Constants.EarthMinVelocity, 0)
    );


List<BodyBase> bodies = new List<BodyBase>();
bodies.Add(Earth);
bodies.Add(Sun);

SolarSystem solarSystem = new SolarSystem(bodies);

solarSystem.Update();

foreach (var body in solarSystem.GetBodies())
{
    Console.WriteLine($"{body.Name} has {body.SurroundedBody.Name}");
}

Earth.UpdateOrbit();

Console.WriteLine(Earth.Orbit.Eccentricity);

app.Run();


//BodyExtend Star = new("Experimental Star", 100000, new Vector(0, 0, 0), new Vector(0, 0, 0));
//BodyExtend Planet = new("Experimental Planet", 1000, new Vector(500, 0, 0), new Vector(0, 5, 0));
//BodyExtend Moon = new("Experimental Moon", 10, new Vector(600, 0, 0), new Vector(0, 10, 0));
//BodyExtend Sattelite = new("Experimental Sattelite", 1, new Vector(650, 0, 0), new Vector(0, 15, 0));

//List<BodyExtend> bodies = new List<BodyExtend>();
//bodies.Add(Sattelite);
//bodies.Add(Planet);
//bodies.Add(Moon);
//bodies.Add(Star);

//SolarSystem solarSystem = new SolarSystem(bodies);

//foreach (var body in solarSystem.GetBodies())
//{
//    Console.WriteLine($"{body.Name}: {body.AbsolutePosition}");
//}

//Console.WriteLine($"Barycentrum: {solarSystem.Barycentrum}");

//solarSystem.CalibrateBarycenterForZero();

//foreach (var body in solarSystem.GetBodies())
//{
//    Console.WriteLine($"{body.Name}: {body.AbsolutePosition}");
//}

//Console.WriteLine($"Barycentrum: {solarSystem.Barycentrum}");


//solarSystem.FindAndAssignSurroundedBody();

//foreach (var body in solarSystem.GetBodies())
//{
//    Console.WriteLine($"{body.Name} has {body.SurroundedBody.Name}");
//}






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
