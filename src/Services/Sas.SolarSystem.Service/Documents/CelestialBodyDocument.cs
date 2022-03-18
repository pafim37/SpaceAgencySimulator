using MongoDB.Bson.Serialization.Attributes;

namespace Sas.SolarSystem.Service.Documents
{
    public class CelestialBodyDocument
    {
        [BsonId]
        public string Name { get; set; }
        public double Mass { get; set; }
        public VectorDocument AbsolutePosition { get; set; }
        public VectorDocument AbsoluteVelocity { get; set; }
        public double Radius { get; set; }
    }
}
