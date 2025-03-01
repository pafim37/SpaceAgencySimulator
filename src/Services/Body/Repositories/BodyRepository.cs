﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Repositories
{
    public class BodyRepository(BodyContext context, IMapper mapper) : IBodyRepository
    {
        public async Task<BodyEntity> CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            BodyEntity? bodyToUpdate = await GetBodyByNameAsync(bodyEntity.Name, cancellationToken).ConfigureAwait(false);
            if (bodyToUpdate is not null)
            {
                throw new BodyAlreadyExistsException($"Body with name {bodyEntity.Name} already exists");
            }
            await context.AddAsync(bodyEntity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return bodyEntity;
        }

        public async Task<BodyEntity> UpdateBodyAsync(BodyDto dataToUpdate, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(dataToUpdate.Name, nameof(dataToUpdate));
            ArgumentNullException.ThrowIfNull(dataToUpdate.Id);
            BodyEntity? bodyEntity = await GetBodyById(dataToUpdate.Id.Value, cancellationToken).ConfigureAwait(false)
                ?? throw new NoBodyInDatabaseException($"There is no body in database with name {dataToUpdate.Name}");
            bodyEntity.Name = dataToUpdate.Name;
            bodyEntity.Mass = dataToUpdate.Mass ?? bodyEntity.Mass;
            bodyEntity.Position = mapper.Map<VectorEntity>(dataToUpdate.Position) ?? bodyEntity.Position;
            bodyEntity.Velocity = mapper.Map<VectorEntity>(dataToUpdate.Velocity) ?? bodyEntity.Velocity;
            bodyEntity.Radius = dataToUpdate.Radius ?? bodyEntity.Radius;
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

        public async Task<IEnumerable<string>> GetAllBodiesNamesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<BodyEntity> bodies = await GetAllBodiesAsync(cancellationToken).ConfigureAwait(false);
            return bodies.Select(b => b.Name);
        }

        public async Task<IEnumerable<BodyEntity>> GetAllEnabledBodiesAsync(CancellationToken cancellationToken)
        {
            return await context.Bodies.Where(body => body.Enabled).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<BodyEntity> ChangeBodyStateAsync(string name, bool newState, CancellationToken cancellationToken)
        {
            BodyEntity? bodyToUpdate = await GetBodyByNameAsync(name, cancellationToken).ConfigureAwait(false)
                ?? throw new NoBodyInDatabaseException($"There is no body in database with name {name}");
            if (bodyToUpdate.Enabled == newState)
            {
                return bodyToUpdate;
            }
            else
            {
                bodyToUpdate.Enabled = newState;
                context.Update(bodyToUpdate);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return bodyToUpdate;
            }
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
