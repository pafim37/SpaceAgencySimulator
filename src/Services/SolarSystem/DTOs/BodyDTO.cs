namespace Sas.BodySystem.Service.DTOs
{
    public class BodyDTO
    {
        public string Name { get; set; }
        public double Mass { get; set; }
        public VectorDTO Position { get; set; }
        public VectorDTO Velocity { get; set; }
        public double Radius { get; set; }
    }
}
