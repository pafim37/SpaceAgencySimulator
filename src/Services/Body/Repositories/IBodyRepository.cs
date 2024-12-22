using Sas.Body.Service.Models;

namespace Sas.Body.Service.Repositories
{
    public interface IBodyRepository
    {
        public Task<IEnumerable<BodyEntity>> GetAllBodiesAsync();
        public Task<BodyEntity?> GetBodyByNameAsync(string name);
        public Task CreateBodyAsync(BodyEntity bodyEntity);
    }
}
