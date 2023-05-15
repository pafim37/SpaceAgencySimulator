using MongoDB.Driver;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.Settings;
namespace Sas.BodySystem.Service.Data
{
    public class BodyContext : IBodyContext
    {
        public IMongoCollection<BodyDocument> CelestialBodies { get; set; }

        public BodyContext(BodyDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            CelestialBodies = database.GetCollection<BodyDocument>(settings.CollectionName);
        }
    }
}
