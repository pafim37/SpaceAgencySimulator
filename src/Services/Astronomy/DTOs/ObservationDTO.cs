using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Astronomy.Service.DTOs
{
    public class ObservationDTO
    {
        public string ObjectName { get; set; }

        public DateTime CreatedOn { get; set; }
        public double AzimuthRad { get; set; }
        public double AltitudeRad { get; set; }
        public double Distance { get; set; }
        public string ObservatoryName { get; set; }
    }
}
