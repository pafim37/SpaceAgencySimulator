using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Service.Astronomy.Models
{
    public class ObservationEntity : Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public double Azimuth { get; set; }

        public double Altitude { get; set; }
        public double Distance { get; set; }

        [ForeignKey("Observatory")]
        public int ObservatoryId { get; set; }
        public ObservatoryEntity Observatory { get; set; }
    }
}
