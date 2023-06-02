namespace Sas.BodySystem.Service.DTOs
{
    public record BodySystemInputData
    {
        public List<BodyDTO> Bodies { get; set; }
        public double GravitationalConstant { get; set; }
    }
}
