using MongoDB.Driver;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.Settings;
namespace Sas.BodySystem.Service.Data
{
    public class SolarSystemContext : ISolarSystemContext
    {
        public IMongoCollection<BodyDocument> CelestialBodies { get; set; }

        public SolarSystemContext(SolarSystemDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            CelestialBodies = database.GetCollection<BodyDocument>(settings.CollectionName);

            bool isDocumentExist = CelestialBodies.Find(p => true).Any();
            if (!isDocumentExist)
            {
                SeedInitializer.Seed(CelestialBodies);
            }
        }

    }
}
