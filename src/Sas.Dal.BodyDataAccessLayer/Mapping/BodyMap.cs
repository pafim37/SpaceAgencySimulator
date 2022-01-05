using Sas.BodySystem.Models;
using Sas.Db.BodyDatabase.Documents;
using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Dal.BodyDataAccessLayer.Mapping
{
    internal class BodyMap
    {
        public Body Map(BodyDocument bodyDocument)
        {
            Vector position = new(bodyDocument.Position.X, bodyDocument.Position.Y, bodyDocument.Position.Z);

            Vector velocity = new(bodyDocument.Velocity.X, bodyDocument.Velocity.Y, bodyDocument.Velocity.Z);

            var body = new Body(bodyDocument.Name, bodyDocument.Mass, position, velocity, bodyDocument.Radius);

            return body;
        } 

        public BodyDocument Map(Body body)
        {
            BodyDocument bodyDocument = new BodyDocument()
            {
                Name = body.Name,
                Mass = body.Mass,
                Radius = body.Radius,
                Position = new VectorDocument
                {
                    X = body.AbsolutePosition.X,
                    Y = body.AbsolutePosition.Y,
                    Z = body.AbsolutePosition.Z
                },
                Velocity = new VectorDocument
                {
                    X = body.AbsoluteVelocity.X,
                    Y = body.AbsoluteVelocity.Y,
                    Z = body.AbsoluteVelocity.Z
                },
            };
            return bodyDocument;
        }

        public IEnumerable<Body> Map(IEnumerable<BodyDocument> bodyDocuments)
        {
            foreach (var bodyDocument in bodyDocuments)
            {
                yield return Map(bodyDocument);
            }
        }

        public IEnumerable<BodyDocument> Map(IEnumerable<Body> bodies)
        {
            foreach (var body in bodies)
            {
                yield return Map(body);
            }
        }
    }
}
