using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.BodyDatabase.Documents
{
    public class BodyDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }

        public VectorDocument Position { get; set; }

        public VectorDocument Velocity { get; set; }
    }
}
