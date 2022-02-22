using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.DAL
{
    public interface ICelestialBodyRepository
    {
        Task<CelestialBodyDocument> GetAsync(string name);
        Task<IEnumerable<CelestialBodyDocument>> GetAsync();
        Task<CelestialBodyDocument> CreateAsync(CelestialBodyDocument body);
        Task UpdateAsync(string name, CelestialBodyDocument body);
        Task RemoveAsync(string name);
    }
}
