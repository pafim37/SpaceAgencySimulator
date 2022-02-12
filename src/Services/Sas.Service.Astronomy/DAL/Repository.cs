using Sas.Service.Astronomy.Data;
using Sas.Service.Astronomy.Models;
using System.Data.Entity;

namespace Sas.Service.Astronomy.DAL
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly Context _context;
        public Repository(Context context)
        {
            _context = context;
        }

        // Read all
        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        
        // Read
        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
