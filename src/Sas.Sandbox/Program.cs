using Sas.SolarSystem;
using Sas.SolarSystem.Orbits;
using Spectre.Console;

SolarSystem solarSystem = new SolarSystem();
solarSystem.Init();

foreach (var body in solarSystem.GetBodies())
{
    Console.WriteLine(body);
}