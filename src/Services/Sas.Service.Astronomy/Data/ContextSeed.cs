using Sas.Service.Astronomy.Models;
using System.Data.Entity;

namespace Sas.Service.Astronomy.Data
{
    internal class ContextSeed : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            IList<ObservatoryEntity> observatories = new List<ObservatoryEntity>();
            IList<ObservationEntity> observations = new List<ObservationEntity>();

            ObservatoryEntity Cracow = new ObservatoryEntity { Id = 1, Name = "Cracow", Latitude = ChangeDegToRad(50.06143), Longitude = ChangeDegToRad(19.93658) };

            observatories.Add(Cracow);
            observatories.Add(new ObservatoryEntity { Id = 1, Name = "Greenwich", Latitude = ChangeDegToRad(51.47781), Longitude = ChangeDegToRad(0.00148) });
            observatories.Add(new ObservatoryEntity { Id = 1, Name = "New York Ford", Latitude = ChangeDegToRad(42.41753), Longitude = ChangeDegToRad(-76.49407) });

            context.Observatories.AddRange(observatories);

            observations.Add(new ObservationEntity { Id = 1, Name = "Moon", ObservatoryId = 1, CreatedOn = new DateTime(2013, 8, 21, 19, 0, 0), Azimuth = ChangeDegToRad(102.15467), Altitude = ChangeDegToRad(3.343), Distance = 367273905, Observatory = Cracow });

            context.Observations.AddRange(observations);
            base.Seed(context);
        }

        private double ChangeDegToRad(double deg)
        {
            return Math.PI * deg / 180;
        }
    }
}
