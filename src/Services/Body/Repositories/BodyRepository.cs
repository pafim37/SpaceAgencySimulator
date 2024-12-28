using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Repositories
{
    public class BodyRepository(BodyContext context, IMapper mapper) : IBodyRepository
    {
        public async Task CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            BodyEntity? bodyToUpdate = await GetBodyByNameAsync(bodyEntity.Name, cancellationToken).ConfigureAwait(false);
            if (bodyToUpdate is not null)
            {
                throw new BodyAlreadyExistsException($"Body with name = {bodyEntity.Name} already exists");
            }
            await context.AddAsync(bodyEntity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateBodyAsync(BodyDto dataToUpdate, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(dataToUpdate.Name, nameof(dataToUpdate));
            BodyEntity? bodyEntity = await GetBodyByNameAsync(dataToUpdate.Name, cancellationToken).ConfigureAwait(false) 
                ?? throw new NoBodyInDatabaseException($"There is no body in database with name = {dataToUpdate.Name}");
            bodyEntity.Mass = dataToUpdate.Mass ?? bodyEntity.Mass;
            bodyEntity.Position = mapper.Map<VectorEntity>(dataToUpdate.Position) ?? bodyEntity.Position;
            bodyEntity.Velocity = mapper.Map<VectorEntity>(dataToUpdate.Velocity) ?? bodyEntity.Velocity;
            bodyEntity.Radius = dataToUpdate.Radius ?? bodyEntity.Radius;
            context.Update(bodyEntity);
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
