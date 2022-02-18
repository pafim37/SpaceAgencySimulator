using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.DAL
{
    public interface IBodyRepository
    {
        Task<BodyDocument> GetAsync(string name);
        Task<IEnumerable<BodyDocument>> GetAsync();
        Task<BodyDocument> CreateAsync(BodyDocument body);
        Task UpdateAsync(string name, BodyDocument body);
        Task RemoveAsync(string name);
    }
}
