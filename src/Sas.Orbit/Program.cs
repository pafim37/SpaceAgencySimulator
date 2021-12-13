// See https://aka.ms/new-console-template for more information
using Sas.Mathematica.Models;
using Sas.Orbit.Bodies;
using Sas.Orbit.Orbits;

Body sun = new()
{
    Mass = Sas.Physics.Constants.SolarMass,
    AbsolutePosition = new Vector(0, 0, 0),
    AbsoluteVelocity = new Vector(0, 0, 0)
};

Body earth = new()
{
    Mass = Sas.Physics.Constants.EarthMass,
    AbsolutePosition = new Vector(1.52098233 * Math.Pow(10, 11), 0, 0),
    AbsoluteVelocity = new Vector(0, 29291, 0)
};

double th = Math.PI / 2;
Console.WriteLine($"initial th: {th}");
Console.WriteLine($"initial c th: {Math.Cos(th)}");
Console.WriteLine($"initial s th: {Math.Sin(th)}");
Console.WriteLine(Math.Cos(th) * 1.52098233 * Math.Pow(10, 11));
Body earth2 = new()
{
    Mass = Sas.Physics.Constants.EarthMass,
    AbsolutePosition = new Vector(Math.Cos(th) * 1.52098233 * Math.Pow(10, 11), Math.Sin(th) * 1.52098233 * Math.Pow(10, 11), 0),
    AbsoluteVelocity = new Vector(Math.Cos(th + Math.PI / 2 ) * 29291, Math.Sin(th + Math.PI / 2 ) * 29291, 0)
};

Body earth3 = new()
{
    Mass = Sas.Physics.Constants.EarthMass,
    AbsolutePosition = new Vector(0, 1.52098233 * Math.Pow(10, 11), 0),
    AbsoluteVelocity = new Vector(-29291, 0, 0)
};

Body spacecraft = new()
{
    Mass = 1000,
    AbsolutePosition = new Vector(1.52098233 * Math.Pow(10, 11) + 400, 0, 0),
    AbsoluteVelocity = new Vector(0, 0, 0)
};


// Console.WriteLine(spacecraft.GetPosition(earth));

GetOrbit getOrbit = new();
var orbit = getOrbit.GetOribt(sun, earth3);
if (orbit.GetType() == typeof(Ellipse))
{
    Ellipse ellipse = (Ellipse)orbit;
    Console.WriteLine( ellipse.RotationalParameter );
}

