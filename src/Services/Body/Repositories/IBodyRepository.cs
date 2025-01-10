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
        public Task CreateBodyAsync(BodyEntity bodyEntity, CancellationToken cancellationToken);
        public Task CreateRangeBodyAsync(List<BodyEntity> bodyEntities, CancellationToken cancellationToken);
        public Task UpdateBodyAsync(BodyDto bodyEntity, CancellationToken cancellationToken);
        public Task ChangeBodyStateAsync(string name, bool newState, CancellationToken cancellationToken);
        public Task DeleteBodyAsync(string name, CancellationToken cancellationToken);
        public Task SaveDatabaseAndSendNotification(CancellationToken cancellationToken);
    }
}
