using Sas.Astronomy.Service.Data;
using Sas.Astronomy.Service.Models;

namespace Sas.Astronomy.Service.DAL
{
    public class ObservationRepository : Repository<ObservationEntity>
    {
        private readonly Context _context;
        public ObservationRepository(Context context) : base(context)
        {
            _context = context;
        }

        // Read 
        public async Task<IEnumerable<ObservationEntity>> GetAsync(string name)
        {
            return _context.Set<ObservationEntity>().Where(x => x.ObjectName.Equals(name)).ToList();
        }
    }
}
