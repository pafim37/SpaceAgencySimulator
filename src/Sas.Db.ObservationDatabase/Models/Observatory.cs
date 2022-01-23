using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.ObservationDatabase.Models
{
    public class Observatory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        
        [ForeignKey("Observations")]
        public ICollection<Observation> Observations { get; set;}
    }
}
