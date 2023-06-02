namespace Sas.BodySystem.Service.DTOs
{
    public class BodySystemOutputData
    {
        public double GravitationalConstant { get; set; }
        public List<BodyDTO> Bodies { get; set; }
        public List<OrbitDTO> Orbits { get; set; }
    }
}
