using MongoDB.Bson.Serialization.Attributes;

namespace Sas.BodySystem.Service.Documents
{
    public class BodyDocument
    {
        [BsonId]
        public string? Name { get; set; }
        public double Mass { get; set; }
        public VectorDocument? Position { get; set; }
        public VectorDocument? Velocity { get; set; }
        public double Radius { get; set; }
    }
}
