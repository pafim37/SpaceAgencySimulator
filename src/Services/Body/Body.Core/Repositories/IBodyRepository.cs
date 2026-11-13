using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Core.Repositories
{
    public interface IBodyRepository
    {
        public Task<IEnumerable<BodyEntity>> GetAllBodiesAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<string>> GetAllBodiesNamesAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<BodyEntity>> GetEnabledBodiesAsync(CancellationToken cancellation);
        public Task<BodyEntity?> GetBodyByNameAsync(string name, CancellationToken cancellationToken);
        public Task<BodyEntity?> GetBodyByNameAsNoTrackingAsync(string name, CancellationToken cancellationToken);
        public Task<BodyEntity?> GetBodyById(int id, CancellationToken cancellationToken);
        public Task<BodyEntity> CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken);
        public Task<List<BodyEntity>> CreateRangeBodyAsync(List<BodyEntity> bodyEntities, CancellationToken cancellationToken);
        public Task<BodyEntity> UpdateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken);
        public Task<BodyEntity> DeleteBodyAsync(string name, CancellationToken cancellationToken);
    }
}
