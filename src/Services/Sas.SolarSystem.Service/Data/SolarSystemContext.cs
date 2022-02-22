using MongoDB.Driver;
using Sas.Mathematica;
using Sas.SolarSystem.Service.Documents;
using Sas.SolarSystem.Service.Settings;

namespace Sas.SolarSystem.Service.Data
{
    public class SolarSystemContext : ISolarSystemContext
    {
        public IMongoCollection<CelestialBodyDocument> CelestialBodies { get; set; }

        public SolarSystemContext(SolarSystemDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            CelestialBodies = database.GetCollection<CelestialBodyDocument>(settings.CollectionName);

            
            bool isDocumentExist = CelestialBodies.Find(p => true).Any();
            if (!isDocumentExist)
            {
                SeedInitializer.Seed(CelestialBodies);
            }
        }

    }
}
