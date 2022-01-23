﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Db.ObservationDatabase.Models
{
    public class Observation
    {
        public int Id { get; set; }

        public string ObjectName { get; set; }

        public DateTime CreatedOn { get; set; }

        public double Azimuth { get; set; }

        public double Altitude { get; set; }
        public double Distance { get; set; }

        [ForeignKey("Observatory")]
        public int ObservatoryId { get; set; }
        public Observatory Observatory { get; set; }
    }
}
