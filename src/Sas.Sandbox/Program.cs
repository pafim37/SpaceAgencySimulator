
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Sas.BodySystem;
//using Sas.BodyDatabase.Database;
Console.WriteLine("HW");
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
