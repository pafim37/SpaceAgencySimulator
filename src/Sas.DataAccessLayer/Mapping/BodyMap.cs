using Sas.BodyDatabase.Entities;
using Sas.BodySystem.Models;
using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.DataAccessLayer.Mapping
{
    public static class BodyMap
    {
        public static BodyEntity Map(Body body)
        {
            BodyEntity entity = new BodyEntity()
            {
                Name = body.Name,
                Mass = body.Mass,
                Radius = body.Radius,
                Position = new PositionEntity
                {
                    X = body.AbsolutePosition.X,
                    Y = body.AbsolutePosition.Y,
                    Z = body.AbsolutePosition.Z
                },
                Velocity = new VelocityEntity
                {
                    X = body.AbsoluteVelocity.X,
                    Y = body.AbsoluteVelocity.Y,
                    Z = body.AbsoluteVelocity.Z
                },
            };
            return entity;
        }

        public static Body Map(BodyEntity bodyEntity)
        {
            if (bodyEntity != null && bodyEntity.Position != null && bodyEntity.Velocity != null && bodyEntity.Name != null)
            {
                var position = new Vector(bodyEntity.Position.X, bodyEntity.Position.Y, bodyEntity.Position.Z);

                var velocity = new Vector(bodyEntity.Velocity.X, bodyEntity.Velocity.Y, bodyEntity.Velocity.Z);

                Body body = new Body(bodyEntity.Name, bodyEntity.Mass, position, velocity, bodyEntity.Radius);
                return body;
            }
            else
                throw new Exception("One or more field are not initialized");
        }

        public static IEnumerable<Body> Map(IEnumerable<BodyEntity> bodyEntities)
        {
            foreach (var bodyEntity in bodyEntities)
            {
                yield return Map(bodyEntity);
            }
        }

        public static IEnumerable<BodyEntity> Map(IEnumerable<Body> bodies)
        {
            foreach (var body in bodies)
            {
                yield return Map(body);
            }
        }
    }
}
