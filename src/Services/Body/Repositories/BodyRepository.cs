using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Repositories
{
    public class BodyRepository(BodyContext context) : IBodyRepository
    {
        public async Task CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            BodyEntity? bodyToUpdate = await GetBodyByNameAsync(bodyEntity.Name, cancellationToken).ConfigureAwait(false);
            if (bodyToUpdate is not null)
            {
                throw new BodyAlreadyExistsException("Body with existing name already exists");
            }
            await context.AddAsync(bodyEntity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            BodyEntity? bodyToUpdate = await GetBodyByNameAsync(bodyEntity.Name, cancellationToken).ConfigureAwait(false) 
                ?? throw new NoBodyInDatabaseException($"There is no body in database with name = {bodyEntity.Name}");
            bodyToUpdate.Name = bodyEntity.Name;
            bodyToUpdate.Mass = bodyEntity.Mass == 0.0 ? bodyToUpdate.Mass : bodyEntity.Mass;
            bodyToUpdate.Position = bodyEntity.Position ?? bodyToUpdate.Position;
            bodyToUpdate.Velocity = bodyEntity.Velocity ?? bodyToUpdate.Velocity;
            bodyToUpdate.Radius = bodyEntity.Radius;
            context.Update(bodyToUpdate);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteBodyAsync(string name, CancellationToken cancellationToken)
        {
            BodyEntity? body = await GetBodyByNameAsync(name, cancellationToken).ConfigureAwait(false);
            if (body != null)
            {
                context.Bodies.Remove(body);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<BodyEntity>> GetAllBodiesAsync(CancellationToken cancellationToken)
        {
            return await context.Bodies.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<BodyEntity?> GetBodyByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await context.Bodies.FirstOrDefaultAsync(b => b.Name == name, cancellationToken).ConfigureAwait(false);
        }
    }
}
