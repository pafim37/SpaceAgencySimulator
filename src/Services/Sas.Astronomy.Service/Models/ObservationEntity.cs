﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Astronomy.Service.Models
{
    public class ObservationEntity : Entity
    {
        public string ObjectName { get; set; }

        public DateTime CreatedOn { get; set; }

        public double AzimuthRad { get; set; }

        public double AltitudeRad { get; set; }
        public double Distance { get; set; }

        [ForeignKey("Observatory")]
        public int ObservatoryId { get; set; }
        public ObservatoryEntity Observatory { get; set; }
    }
}
