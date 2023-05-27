using Sas.Domain.Models.Orbits;

namespace Sas.BodySystem.Service.DTOs
{
    public class BodySystemDTO
    {
        public List<BodyDTO> Bodies { get; set; }
        public List<OrbitDTO> Orbits { get; set; }
    }
}
