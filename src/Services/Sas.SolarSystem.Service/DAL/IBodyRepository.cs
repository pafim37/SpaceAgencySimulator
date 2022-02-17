using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.DAL
{
    public interface IBodyRepository
    {
        Task<BodyDocument> GetAsync(string name);
        Task<IEnumerable<BodyDocument>> GetAsync();
        //Task<BodyDocument> CreateAsync(Body body);
        //Task UpdateAsync(string name, Body body);
        //Task RemoveAsync(string name);
    }
}
