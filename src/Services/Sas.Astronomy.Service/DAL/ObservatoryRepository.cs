using Sas.Astronomy.Service.Data;
using Sas.Astronomy.Service.Models;

namespace Sas.Astronomy.Service.DAL
{
    public class ObservatoryRepository : Repository<ObservatoryEntity>
    {
        private readonly Context _context;
        public ObservatoryRepository(Context context) : base(context)
        {
            _context = context;
        }

        // Read 
        public async Task<ObservatoryEntity> GetAsync(string name)
        {
            return _context.Set<ObservatoryEntity>().Where(x => x.Name.Equals(name)).FirstOrDefault();
        }
    }
}
