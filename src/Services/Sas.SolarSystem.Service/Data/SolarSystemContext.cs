using MongoDB.Driver;
using Sas.Mathematica;
using Sas.SolarSystem.Service.Documents;
using Sas.SolarSystem.Service.Settings;

namespace Sas.SolarSystem.Service.Data
{
    public class SolarSystemContext : ISolarSystemContext
    {
        public IMongoCollection<BodyDocument> Bodies { get; set; }

        public SolarSystemContext(BodyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Bodies = database.GetCollection<BodyDocument>(settings.CollectionName);

            SeedData();
        }
        
        private void SeedData()
        {
            BodyDocument Sun = new() {
                Name = "Sun",
                Mass = Constants.SolarMass,
                AbsolutePosition = new VectorDocument { X = 0, Y = 0, Z = 0 },
                AbsoluteVelocity = new VectorDocument { X = 0, Y = 0, Z = 0 },
                Radius = Constants.SunRadius
            };

            BodyDocument Earth = new()
            {
                Name = "Earth",
                Mass = Constants.EarthMass,
                AbsolutePosition = new VectorDocument { X = Constants.EarthApoapsis, Y = 0, Z = 0 },
                AbsoluteVelocity = new VectorDocument { X = 0, Y = Constants.EarthMinVelocity, Z = 0 },
                Radius = Constants.EarthRadius
            };

            BodyDocument Sattelite = new()
            {
                Name = "Sattelite",
                Mass = 100,
                AbsolutePosition = new VectorDocument { X = Constants.EarthRadius + 400, Y = 0, Z = 0 },
                AbsoluteVelocity = new VectorDocument { X = 0, Y = 500, Z = 0 },
            };

            Bodies.InsertOne(Sattelite);
            Bodies.InsertOne(Sun);
            Bodies.InsertOne(Earth);
        }
    }
}
