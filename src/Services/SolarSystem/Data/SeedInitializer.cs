using MongoDB.Driver;
using Sas.BodySystem.Service.Documents;
using Sas.Mathematica.Service;

namespace Sas.BodySystem.Service.Data
{
    internal class SeedInitializer
    {
        public static void Seed(IMongoCollection<BodyDocument> bodies)
        {
            BodyDocument Sun = new()
            {
                Name = "Sun",
                Mass = Constants.SolarMass,
                Position = new VectorDocument { X = 0, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = 0, Z = 0 },
                Radius = Constants.SunRadius
            };

            BodyDocument Earth = new()
            {
                Name = "Earth",
                Mass = Constants.EarthMass,
                Position = new VectorDocument { X = Constants.EarthApoapsis, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = Constants.EarthMinVelocity, Z = 0 },
                Radius = Constants.EarthRadius
            };

            BodyDocument Moon = new()
            {
                Name = "Moon",
                Mass = Constants.MoonMass,
                Position = new VectorDocument { X = Constants.MoonPeriapsis + Constants.EarthApoapsis, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = Constants.MoonMaxVelocity + Constants.EarthMinVelocity, Z = 0 },
                Radius = Constants.MoonRadius
            };

            BodyDocument EarthSattelite1 = new()
            {
                Name = "Earth Sattelite 1",
                Mass = 100,
                Position = new VectorDocument { X = Constants.EarthApoapsis + Constants.EarthRadius, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = 500, Z = 0 },
            };

            BodyDocument EarthSattelite2 = new()
            {
                Name = "Earth Sattelite 2",
                Mass = 100,
                Position = new VectorDocument { X = Constants.EarthApoapsis - Constants.EarthRadius, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = 500, Z = 0 },
            };

            BodyDocument MoonSattelite1 = new()
            {
                Name = "Moon Sattelite 1",
                Mass = 100,
                Position = new VectorDocument { X = Constants.MoonPeriapsis + Constants.EarthApoapsis + Constants.MoonRadius, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = 500, Z = 0 },
            };

            BodyDocument MoonSattelite2 = new()
            {
                Name = "Moon Sattelite 2",
                Mass = 100,
                Position = new VectorDocument { X = Constants.MoonPeriapsis + Constants.EarthApoapsis - Constants.MoonRadius, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = 500, Z = 0 },
            };

            BodyDocument SunSattelite = new()
            {
                Name = "Sun Sattelite",
                Mass = 100,
                Position = new VectorDocument { X = Constants.SunRadius, Y = 0, Z = 0 },
                Velocity = new VectorDocument { X = 0, Y = 500, Z = 0 },
            };

            bodies.InsertOne(Sun);
            bodies.InsertOne(Earth);
            bodies.InsertOne(Moon);
            bodies.InsertOne(EarthSattelite1);
            bodies.InsertOne(EarthSattelite2);
            bodies.InsertOne(MoonSattelite1);
            bodies.InsertOne(MoonSattelite2);
            bodies.InsertOne(SunSattelite);
        }
    }
}
