using Sas.Astronomy.Service.Models;
using System.Data.Entity;

namespace Sas.Astronomy.Service.Data
{
    internal class ContextSeed : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            IList<ObservatoryEntity> observatories = new List<ObservatoryEntity>();
            IList<ObservationEntity> observations = new List<ObservationEntity>();

            ObservatoryEntity Cracow = new ObservatoryEntity { Id = 1, Name = "Cracow", LatitudeRad = ChangeDegToRad(50.06143), LongitudeRad = ChangeDegToRad(19.93658), Height = 219 };
            ObservatoryEntity Greenwich = new ObservatoryEntity { Id = 2, Name = "Greenwich", LatitudeRad = ChangeDegToRad(51.47781), LongitudeRad = ChangeDegToRad(0.00148), Height = 0 };
            ObservatoryEntity Catalina = new ObservatoryEntity { Id = 3, Name = "Catalina", LatitudeRad = ChangeDegToRad(32.417), LongitudeRad = ChangeDegToRad(-110.732), Height = 2509 };

            observatories.Add(Cracow);
            observatories.Add(Greenwich);
            observatories.Add(new ObservatoryEntity { Id = 4, Name = "New York Ford", LatitudeRad = ChangeDegToRad(42.41753), LongitudeRad = ChangeDegToRad(-76.49407), Height = 348});
            observatories.Add(Catalina);
            
            context.Observatories.AddRange(observatories);

            observations.Add(new ObservationEntity { Id = 1, ObjectName = "Moon", ObservatoryId = 1, CreatedOn = new DateTime(2013, 8, 21, 19, 0, 0), AzimuthRad = ChangeDegToRad(102.15467), AltitudeRad = ChangeDegToRad(3.343), Distance = 367273905, Observatory = Cracow });
            observations.Add(new ObservationEntity { Id = 2, ObjectName = "Moon", ObservatoryId = 1, CreatedOn = new DateTime(2013, 8, 21, 20, 0, 0), AzimuthRad = ChangeDegToRad(109.24953), AltitudeRad = ChangeDegToRad(8.868778), Distance = 367004977, Observatory = Greenwich });
            observations.Add(new ObservationEntity { Id = 3, ObjectName = "Artificial Sattelite", ObservatoryId = 3, CreatedOn = new DateTime(2004, 4, 5, 19, 0, 0), AzimuthRad = ChangeDegToRad(128), AltitudeRad = ChangeDegToRad(41), Distance = 27148, Observatory = Catalina });

            context.Observations.AddRange(observations);
            base.Seed(context);
        }

        private double ChangeDegToRad(double deg)
        {
            return Math.PI * deg / 180;
        }
    }
}
