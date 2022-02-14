using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Astronomy.Service.DTOs
{
    public class ObservatoryDTO
    {
        public string Name { get; set; }

        public double LongitudeRad { get; set; }
        public double LatitudeRad { get; set; }
        public double Height { get; set; }
    }
}
