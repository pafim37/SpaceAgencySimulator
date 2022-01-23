using Sas.Db.ObservationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.ObservationDatabase.Data
{
    internal class ObservationDbInitializer : CreateDatabaseIfNotExists<ObservationDb>
    {
        protected override void Seed(ObservationDb context)
        {
            IList<Observatory> observatories = new List<Observatory>();
            IList<Observation> observations = new List<Observation>();

            Observatory Cracow = new Observatory { Id = 1, Name = "Cracow", Latitude = ChangeDegToRad(50.06143), Longitude = ChangeDegToRad(19.93658) };

            observatories.Add(Cracow);
            observatories.Add(new Observatory { Id = 1, Name = "Greenwich", Latitude = ChangeDegToRad(51.47781), Longitude = ChangeDegToRad(0.00148) });
            observatories.Add(new Observatory { Id = 1, Name = "New York Ford", Latitude = ChangeDegToRad(42.41753), Longitude = ChangeDegToRad(-76.49407) });

            context.Observatories.AddRange(observatories);

            observations.Add(new Observation { Id = 1, ObjectName = "Moon", ObservatoryId = 1, CreatedOn = new DateTime(2013, 8, 21, 19, 0, 0), Azimuth = ChangeDegToRad(102.15467), Altitude = ChangeDegToRad(3.343), Distance = 367273905, Observatory = Cracow });

            context.Observations.AddRange(observations);
            base.Seed(context);
        }

        private double ChangeDegToRad(double deg)
        {
            return Math.PI * deg / 180;
        }
    }
}
