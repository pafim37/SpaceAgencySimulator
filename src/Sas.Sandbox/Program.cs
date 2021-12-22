using Sas.Mathematica;
using Sas.SolarSystem;
using Sas.SolarSystem.Models;
using Sas.SolarSystem.Orbits;
using Spectre.Console;

//SolarSystem solarSystem = new SolarSystem();
//solarSystem.Init();

//foreach (var body in solarSystem.GetBodies())
//{
//    if (body.Name.Equals("Earth"))
//    {
//        Console.WriteLine($"{body.Name}, {body.U}, {body.Orbit.SemiLatusRectum}");
//    }
//}

Body body1 = new Body("1", 1, new Vector(1, 0, 0), Vector.Zero);
Body body2 = new Body("2", 1, new Vector(0, 1, 0), Vector.Zero);
Body body3 = new Body("3", 1, new Vector(0, 0, 1), Vector.Zero);

SolarSystem mySolarSystem = new SolarSystem();
mySolarSystem.AddBody(body1);
mySolarSystem.AddBody(body2);
mySolarSystem.AddBody(body3);

var bc = mySolarSystem.GetBarycentrum();
Console.WriteLine(bc);