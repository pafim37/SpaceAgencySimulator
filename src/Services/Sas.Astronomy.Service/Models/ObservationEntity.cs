using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Astronomy.Service.Models
{
    public class ObservationEntity : Entity
    {
        [Required]
        public string ObjectName { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public double AzimuthRad { get; set; }
        [Required]
        public double AltitudeRad { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        [ForeignKey("Observatory")]
        public int ObservatoryId { get; set; }
        public ObservatoryEntity Observatory { get; set; }
    }
}
