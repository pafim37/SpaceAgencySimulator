using MongoDB.Driver;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.Data
{
    public interface IBodyDatabase
    {
        IMongoCollection<BodyDocument> Bodies { get; set; }
    }
}
