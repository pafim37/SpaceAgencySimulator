using MongoDB.Driver;
using Sas.BodySystem.Service.Data;
using Sas.BodySystem.Service.Documents;

namespace Sas.BodySystem.Service.DAL
{
    public class BodyRepository : IBodyRepository
    {
        private readonly ISolarSystemContext _context;

        public BodyRepository(ISolarSystemContext context)
        {
            _context = context;
        }

        // Read
        public async Task<BodyDocument> GetAsync(string name)
        {
            var body = await _context.CelestialBodies.Find(b => b.Name.Equals(name)).FirstOrDefaultAsync();
            return body;
        }

        // Read All
        public async Task<IEnumerable<BodyDocument>> GetAsync()
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

        // Update
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
