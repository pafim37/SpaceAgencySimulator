namespace Sas.Astronomy.Service.DTOs
{
    public class ObservationCreateInstantDTO
    {
        public string ObjectName { get; set; }
        public double AzimuthRad { get; set; }
        public double AltitudeRad { get; set; }
        public double Distance { get; set; }
    }
}
