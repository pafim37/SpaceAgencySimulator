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

        // Read 
        public async Task<T> GetAsync(string name)
        {
            return _context.Set<T>().Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        // Read all
        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
