namespace Sas.Body.Service.DataTransferObject
{
    public class BodyDto
    {
        public string? Name { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public VectorDto? Position { get; set; }
        public VectorDto? Velocity { get; set; }
    }
}
