using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.Models;

namespace Sas.Body.Service.Repositories
{
    public class BodyRepository(BodyContext context) : IBodyRepository
    {
        public async Task CreateBodyAsync(BodyEntity bodyEntity)
        {
            await context.AddAsync(bodyEntity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<BodyEntity>> GetAllBodiesAsync()
        {
            return await context.Bodies.ToListAsync().ConfigureAwait(false);
        }

        public async Task<BodyEntity?> GetBodyByNameAsync(string name)
        {
            return await context.Bodies.FirstOrDefaultAsync(b => b.Name == name).ConfigureAwait(false);
        }
    }
}
