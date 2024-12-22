using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Repositories
{
    public class BodyRepository(BodyContext context) : IBodyRepository
    {
        public async Task CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken)
        {
            await context.AddAsync(bodyEntity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<BodyEntity>> GetAllBodiesAsync(CancellationToken cancellationToken)
        {
            return await context.Bodies.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<BodyEntity?> GetBodyByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await context.Bodies.FirstOrDefaultAsync(b => b.Name == name, cancellationToken).ConfigureAwait(false);
        }
    }
}
