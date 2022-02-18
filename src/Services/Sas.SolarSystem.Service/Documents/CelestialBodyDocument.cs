using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sas.SolarSystem.Service.Documents
{
    public class CelestialBodyDocument : BodyDocument
    {
        public double Radius { get; set; }
    }
}
