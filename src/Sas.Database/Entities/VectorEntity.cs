using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.BodyDatabase.Entities
{
    public class VectorEntity : Entity
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        //[ForeignKey{"BodyEntity")]
        //public int BodyEntityId { get; set; }
    }
}
