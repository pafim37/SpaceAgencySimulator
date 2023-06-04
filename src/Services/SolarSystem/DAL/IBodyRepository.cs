using Sas.BodySystem.Service.Documents;

namespace Sas.BodySystem.Service.DAL
{
    public interface IBodyRepository
    {
        Task<BodyDocument> GetAsync(string name);
        Task<IEnumerable<BodyDocument>> GetAllAsync();
        Task<BodyDocument> CreateAsync(BodyDocument body);
        Task<IEnumerable<BodyDocument>> CreateAsync(IEnumerable<BodyDocument> bodies);
        Task<IEnumerable<BodyDocument>> CreateOrReplaceAsync(IEnumerable<BodyDocument> bodies);
        Task ReplaceAsync(string name, BodyDocument body);
        Task RemoveAsync(string name);
        Task RemoveManyAsync(IEnumerable<string?> names);
    }
}
