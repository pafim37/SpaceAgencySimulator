using MongoDB.Driver;
using Sas.BodySystem.Service.Documents;

namespace Sas.BodySystem.Service.Data
{

    public interface IBodyContext
    {
        IMongoCollection<BodyDocument> CelestialBodies { get; set; }
    }
}
