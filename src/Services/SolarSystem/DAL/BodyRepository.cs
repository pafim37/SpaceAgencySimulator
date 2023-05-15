using MongoDB.Driver;
using Sas.BodySystem.Service.Data;
using Sas.BodySystem.Service.Documents;

namespace Sas.BodySystem.Service.DAL
{
    public class BodyRepository : IBodyRepository
    {
        private readonly IBodyContext _context;

        public BodyRepository(IBodyContext context)
        {
            _context = context;
        }

        // Read
        public async Task<BodyDocument> GetAsync(string name)
        {
            BodyDocument body = await _context.CelestialBodies.Find(b => b.Name.Equals(name)).FirstOrDefaultAsync();
            return body;
        }

        // Read All
        public async Task<IEnumerable<BodyDocument>> GetAllAsync()
        {
            var bodies = await _context.CelestialBodies.Find(b => true).ToListAsync();
            return bodies;
        }

        // Create
        public async Task<BodyDocument> CreateAsync(BodyDocument body)
        {
            await _context.CelestialBodies.InsertOneAsync(body);
            return body;
        }

        // Create many
        public async Task<IEnumerable<BodyDocument>> CreateAsync(IEnumerable<BodyDocument> bodies)
        {
            await _context.CelestialBodies.InsertManyAsync(bodies);
            return bodies;
        }

        // Create or Update
        public async Task<IEnumerable<BodyDocument>> CreateOrUpdateAsync(IEnumerable<BodyDocument> bodies)
        {
            foreach (BodyDocument body in bodies)
            {
                BodyDocument bodyToUpdate = await GetAsync(body.Name).ConfigureAwait(false);
                if(bodyToUpdate != null)
                {
                    await UpdateAsync(body.Name, body).ConfigureAwait(false);
                }
                else
                {
                    await CreateAsync(body).ConfigureAwait(false);
                }
            }
            return bodies;
        }

        // Update
        // TODO: Consider between Update and Replace
        public async Task UpdateAsync(string name, BodyDocument body)
        {
            await _context.CelestialBodies.ReplaceOneAsync(b => b.Name.Equals(name), body);
        }

        // Remove
        public async Task RemoveAsync(string name)
        {
            await _context.CelestialBodies.DeleteOneAsync(b => b.Name.Equals(name));
        }
    }
}
