using MongoDB.Driver;
using Sas.SolarSystem.Service.Data;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.DAL
{
    public class CelestialBodyRepository : ICelestialBodyRepository
    {
        private readonly ISolarSystemContext _context;

        public CelestialBodyRepository(ISolarSystemContext context)
        {
            _context = context;
        }

        // Read
        public async Task<CelestialBodyDocument> GetAsync(string name)
        {
            var body = await _context.CelestialBodies.Find(b => b.Name.Equals(name)).FirstOrDefaultAsync();
            return body;
        }

        // Read All
        public async Task<IEnumerable<CelestialBodyDocument>> GetAsync()
        {
            var bodies = await _context.CelestialBodies.Find(b => true).ToListAsync();
            return bodies;
        }

        // Create
        public async Task<CelestialBodyDocument> CreateAsync(CelestialBodyDocument body)
        {
            await _context.CelestialBodies.InsertOneAsync(body);
            return body;
        }

        // Update
        public async Task UpdateAsync(string name, CelestialBodyDocument body)
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
