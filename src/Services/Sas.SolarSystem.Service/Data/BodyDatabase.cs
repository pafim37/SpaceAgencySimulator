using MongoDB.Driver;
using Sas.SolarSystem.Service.Documents;
using Sas.SolarSystem.Service.Settings;

namespace Sas.SolarSystem.Service.Data
{
    public class BodyDatabase : IBodyDatabase
    {
        public IMongoCollection<BodyDocument> Bodies { get; set; }

        public BodyDatabase(BodyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Bodies = database.GetCollection<BodyDocument>(settings.CollectionName);
            // SeedData(); 
        }

        private void SeedData()
        {
            var b1 = new BodyDocument()
            {
                Name = "Body",
                Mass = 1,
                AbsolutePosition = new VectorDocument() { X = 1, Y = 1, Z = 1 },
                AbsoluteVelocity = new VectorDocument() { X = 1, Y = 1, Z = 1 }
            };

            Bodies.InsertOne(b1);
        }
    }
}
