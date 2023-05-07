using MongoDB.Driver;
using Sas.BodySystem.Service.Documents;

namespace Sas.BodySystem.Service.Data
{

    public interface ISolarSystemContext
    {
        IMongoCollection<BodyDocument> CelestialBodies { get; set; }
    }
}
