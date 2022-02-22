using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sas.SolarSystem.Service.Documents
{
    public class CelestialBodyDocument
    {
        [BsonId]
        public string Name { get; set; }
        public double Mass { get; set; }
        public CelestialVectorDocument AbsolutePosition { get; set; }
        public CelestialVectorDocument AbsoluteVelocity { get; set; }
        public double Radius { get; set; }
    }
}
