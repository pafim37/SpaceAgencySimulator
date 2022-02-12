using Sas.Service.Astronomy.Data;
using Sas.Service.Astronomy.Models;

namespace Sas.Service.Astronomy.DAL
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
