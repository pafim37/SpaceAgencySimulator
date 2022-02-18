using MongoDB.Driver;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.Data
{
    public interface ISolarSystemContext
    {
        IMongoCollection<BodyDocument> Bodies { get; set; }
    }
}
