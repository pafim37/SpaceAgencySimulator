using MediatR;
using Sas.Body.Business.Models.Domain.Bodies;
using Sas.Body.Business.Models.Domain.BodySystems;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.Repositories;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Business.Handlers
{
    public class CreateBodySystemHandler(IBodyRepository bodyRepository) : IRequestHandler<CreateBodySystem, BodySystem>
    {
        public async Task<BodySystem> Handle(CreateBodySystem request, CancellationToken cancellationToken)
        {
            IEnumerable<BodyEntity> bodiesDto = await bodyRepository.GetAllEnabledBodiesAsync(cancellationToken).ConfigureAwait(false);
            List<BodyDomain> bodyDomains = [];
            foreach (BodyEntity bodyEntity in bodiesDto)
            {
                VectorEntity position = bodyEntity.Position;
                VectorEntity velocity = bodyEntity.Velocity;
                bodyDomains.Add(
                    new BodyDomain(
                        bodyEntity.Name,
                        bodyEntity.Mass,
                        new Vector(position.X, position.Y, position.Z),
                        new Vector(velocity.X, velocity.Y, velocity.Z),
                        bodyEntity.Radius
                    )
                );
            }
            BodySystem bodySystem = new(bodyDomains);
            bodySystem.UpdateBodySystem();
            bodySystem.FindOrbits(request.SkipStaticPoints);
            return bodySystem;
        }
    }
}
