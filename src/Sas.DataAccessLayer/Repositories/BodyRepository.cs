using Microsoft.EntityFrameworkCore;
using Sas.BodyDatabase.Database;
using Sas.BodyDatabase.Entities;
using Sas.BodySystem.Models;
using Sas.DataAccessLayer.Mapping;

namespace Sas.DataAccessLayer.Repositories
{
    public class BodyRepository
    {
        private readonly BodyContext _context;
        public BodyRepository(BodyContext context)
        {
            _context = context;
        }

        public async Task<Body> GetBodyByNameAsync(string name)
        {
            var bodyEntity = await GetByNameAsync(name);
            if (bodyEntity != null)
            {
                await AssignVectors(bodyEntity);
                return BodyMap.Map(bodyEntity);
            }
            else
            {
                throw new Exception($"{name}");
            }
        }

        public async Task<IEnumerable<Body>> GetAllBodiesAsync()
        {
            List<Body> result = new List<Body>();
            var bodyEntities = await GetAllAsync();
            foreach (var bodyEntity in bodyEntities)
            {
                await AssignVectors(bodyEntity);
                var body = BodyMap.Map(bodyEntity);
                result.Add(body);
            }
            return result;
        }

        private async Task<BodyEntity> GetByNameAsync(string name)
        {
            var body = await _context.Bodies.Where(b => b.Name == name).FirstOrDefaultAsync();
            if (body != null)
            {
                await AssignVectors(body);
                return body;
            }
            else
                throw new Exception($"No body {name} exists");
        }

        private async Task<IEnumerable<BodyEntity>> GetAllAsync()
        {
            var bodyEntities = await _context.Bodies.ToListAsync();
            return bodyEntities;
        }

        private async Task AssignVectors(BodyEntity bodyEntity)
        {
            var position = await _context.Positions.Where(p => p.Id == bodyEntity.PositionId).FirstOrDefaultAsync();
            var velocity = await _context.Velocities.Where(v => v.Id == bodyEntity.VelocityId).FirstOrDefaultAsync();
            if (position != null && velocity != null)
            {
                bodyEntity.Position = position;
                bodyEntity.Velocity = velocity;
            }
            else
                throw new Exception($"Position or velocity for {bodyEntity.Name} not found");
        }
    }
}
