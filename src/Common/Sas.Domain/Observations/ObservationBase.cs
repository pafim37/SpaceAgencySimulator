using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain.Observations
{
    public class ObservationBase
    {
        /// <summary>
        /// Gets observatory that create the observation
        /// </summary>
        public Observatory Observatory { get; set; }

        /// <summary>
        /// Gets name of the observed object
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Local date of creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

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
            CreatedOn = createdOn;
        }
    }
}
