using Sas.Astronomy.Service.Data;
using Sas.Astronomy.Service.Models;
using System.Data.Entity;

namespace Sas.Astronomy.Service.DAL
{
    public class ObservatoryRepository
    {
        private readonly Context _context;
        public ObservatoryRepository(Context context)
        {
            _context = context;
        }

        // Read all
        public async Task<IEnumerable<ObservatoryEntity>> GetAsync()
        {
            return await _context.Set<ObservatoryEntity>().ToListAsync();
        }

        // Read by id
        public async Task<ObservatoryEntity> GetAsync(int id)
        {
            return await _context.Set<ObservatoryEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Read by name
        public async Task<ObservatoryEntity> GetAsync(string name)
        {
            return await _context.Set<ObservatoryEntity>().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
