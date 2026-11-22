using Sas.BodySystem.Process.Models.Domain.Orbits;

namespace Sas.BodySystem.Process.Models.DataTransferObject
{
    using BodyDomain = Domain.Bodies.BodyDomain;
    public class BodySystemDto
    {
        public BodySystemDto()
        {
            // Default constructor for serialization
        }
        public double GravitationalConstant { get; set; }
        public BodyDomain? Barycentrum { get; set; }
        public IReadOnlyList<BodyDomain> Bodies { get; set; } = [];
        public List<OrbitDto> Orbits { get; set; } = [];
    }
}
