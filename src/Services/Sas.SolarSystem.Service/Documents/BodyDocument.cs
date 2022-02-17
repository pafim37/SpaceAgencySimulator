using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sas.SolarSystem.Service.Documents
{
    public class BodyDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public VectorDocument AbsolutePosition { get; set; }
        public VectorDocument AbsoluteVelocity { get; set; }
    }
}
