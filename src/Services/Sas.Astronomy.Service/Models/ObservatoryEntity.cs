using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Astronomy.Service.Models
{
    public class ObservatoryEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }
      
        [Required]
        public double LongitudeRad { get; set; }
        [Required]
        public double LatitudeRad { get; set; }
        [Required]
        public double Height { get; set; }
        [ForeignKey("Observations")]
        public ICollection<ObservationEntity>? Observations { get; set; }
    }
}
