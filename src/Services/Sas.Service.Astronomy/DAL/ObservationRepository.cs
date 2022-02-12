using Sas.Service.Astronomy.Data;
using Sas.Service.Astronomy.Models;

namespace Sas.Service.Astronomy.DAL
{
    public class ObservationRepository : Repository<ObservationEntity>
    {
        private readonly Context _context;
        public ObservationRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
