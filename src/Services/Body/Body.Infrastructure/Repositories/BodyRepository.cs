using Sas.Body.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Sas.Body.Infrastructure.Contexts;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Infrastructure.Repositories
{
    public class BodyRepository(BodyContext context) : IBodyRepository
    {
        public async Task<BodyEntity> CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            await context.AddAsync(bodyEntity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return bodyEntity;
        }

        public async Task<BodyEntity> UpdateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            context.Update(bodyEntity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return bodyEntity;
        }
        public async Task<BodyEntity> DeleteBodyAsync(string name, CancellationToken cancellationToken)
        {
            BodyEntity? body = await GetBodyByNameAsync(name, cancellationToken).ConfigureAwait(false);
            if (body != null)
            {
                context.Bodies.Remove(body);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return body;
            }
            else
            {
                throw new NoBodyInDatabaseException($"There is no body in database with name {name}");
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

        public async Task<BodyEntity?> GetBodyByNameAsNoTrackingAsync(string name, CancellationToken cancellationToken)
        {
            return await context.Bodies.AsNoTracking().FirstOrDefaultAsync(b => b.Name == name, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> GetAllBodiesNamesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<BodyEntity> bodies = await GetAllBodiesAsync(cancellationToken).ConfigureAwait(false);
            return bodies.Select(b => b.Name);
        }

        public async Task<IEnumerable<BodyEntity>> GetAllEnabledBodiesAsync(CancellationToken cancellationToken)
        {
            return await context.Bodies.Where(body => body.Enabled).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<BodyEntity>> CreateRangeBodyAsync(List<BodyEntity> bodyEntities, CancellationToken cancellationToken)
        {
            await context.AddRangeAsync(bodyEntities, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return bodyEntities;
        }

        public async Task<BodyEntity?> GetBodyById(int id, CancellationToken cancellationToken)
        {
            return await context.Bodies.FirstOrDefaultAsync(b => b.Id == id, cancellationToken).ConfigureAwait(false);
        }
    }
}
