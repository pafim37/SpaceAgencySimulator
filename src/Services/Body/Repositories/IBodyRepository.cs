using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Repositories
{
    public interface IBodyRepository
    {
        public Task<IEnumerable<BodyEntity>> GetAllBodiesAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<string>> GetAllBodiesNamesAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<BodyEntity>> GetAllEnabledBodiesAsync(CancellationToken cancellation);
        public Task<BodyEntity?> GetBodyByNameAsync(string name, CancellationToken cancellationToken);
        public Task<BodyEntity?> GetBodyById(int id, CancellationToken cancellationToken);
        public Task<BodyEntity> CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken);
        public Task<List<BodyEntity>> CreateRangeBodyAsync(List<BodyEntity> bodyEntities, CancellationToken cancellationToken);
        public Task<BodyEntity> UpdateBodyAsync(BodyDto bodyEntity, CancellationToken cancellationToken);
        public Task<BodyEntity> ChangeBodyStateAsync(string name, bool newState, CancellationToken cancellationToken);
        public Task<BodyEntity> DeleteBodyAsync(string name, CancellationToken cancellationToken);
    }
}
