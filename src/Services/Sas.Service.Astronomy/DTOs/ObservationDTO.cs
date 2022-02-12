using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Service.Astronomy.DTOs
{
    public class ObservationDTO
    {
        public string ObjectName { get; set; }
        public DateTime CreatedOn { get; set; }
        public double Azimuth { get; set; }
        public double Altitude { get; set; }
        public double Distance { get; set; }
        public string ObservatoryName { get; set; }
    }
}
