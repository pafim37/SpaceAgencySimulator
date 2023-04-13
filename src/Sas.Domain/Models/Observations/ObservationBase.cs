using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sas.Domain.Models.Clocks;
using Sas.Domain.Models.Observatories;

namespace Sas.Domain.Models.Observations
{
    public class ObservationBase
    {
        /// <summary>
        /// Gets name of the observed object
        /// </summary>
        public string ObjectName { get; }

        /// <summary>
        /// Gets observatory that create the observation
        /// </summary>
        public Observatory Observatory { get; }

        /// <summary>
        /// Time of the creation
        /// </summary>
        public AstronomicalClock Time { get; }

        /// <summary>
        /// Constructor of the ObservationBase
        /// </summary>
        /// <param name="observatory"></param>
        /// <param name="objectName"></param>
        /// <param name="createdOn"></param>
        /// <param name=""></param>
        public ObservationBase(Observatory observatory, string objectName, DateTime createdOn)
        {
            Observatory = observatory;
            ObjectName = objectName;
            Time = new AstronomicalClock(createdOn, observatory.LongitudeRad);
        }

    }
}
