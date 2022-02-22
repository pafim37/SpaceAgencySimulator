using MongoDB.Driver;
using Sas.Mathematica;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.Data
{
    internal class SeedInitializer
    {
        public static void Seed(IMongoCollection<CelestialBodyDocument> bodies)
        {
            CelestialBodyDocument Sun = new()
            {
                Name = "Sun",
                Mass = Constants.SolarMass,
                AbsolutePosition = new CelestialVectorDocument { X = 0, Y = 0, Z = 0 },
                AbsoluteVelocity = new CelestialVectorDocument { X = 0, Y = 0, Z = 0 },
                Radius = Constants.SunRadius
            };

            CelestialBodyDocument Earth = new()
            {
                Name = "Earth",
                Mass = Constants.EarthMass,
                AbsolutePosition = new CelestialVectorDocument { X = Constants.EarthApoapsis, Y = 0, Z = 0 },
                AbsoluteVelocity = new CelestialVectorDocument { X = 0, Y = Constants.EarthMinVelocity, Z = 0 },
                Radius = Constants.EarthRadius
            };

            CelestialBodyDocument Moon = new()
            {
                Name = "Moon",
                Mass = Constants.MoonMass,
                AbsolutePosition = new CelestialVectorDocument { X = Constants.MoonPeriapsis + Constants.EarthApoapsis, Y = 0, Z = 0 },
                AbsoluteVelocity = new CelestialVectorDocument { X = 0, Y = Constants.MoonMaxVelocity + Constants.EarthMinVelocity, Z = 0 },
                Radius = Constants.MoonRadius
            };

            CelestialBodyDocument Sattelite = new()
            {
                Name = "Sattelite",
                Mass = 100,
                AbsolutePosition = new CelestialVectorDocument { X = Constants.EarthRadius + 400, Y = 0, Z = 0 },
                AbsoluteVelocity = new CelestialVectorDocument { X = 0, Y = 500, Z = 0 },
            };

            bodies.InsertOne(Sun);
            bodies.InsertOne(Earth);
            bodies.InsertOne(Moon);
            bodies.InsertOne(Sattelite);
        }
    }
}
