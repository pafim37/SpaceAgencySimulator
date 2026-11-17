using Microsoft.EntityFrameworkCore;
using Sas.Body.Core.Models.Entities;
using Sas.Mathematica.Service;

namespace Sas.Body.Infrastructure.Contexts
{
    public class SeedInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            VectorEntity sunPosition = new()
            {
                Id = 1,
                X = 0,
                Y = 0,
                Z = 0,
            };

            VectorEntity sunVelocity = new()
            {
                Id = 2,
                X = 0,
                Y = 0,
                Z = 0,
            };

            modelBuilder.Entity<VectorEntity>().HasData(sunPosition);
            modelBuilder.Entity<VectorEntity>().HasData(sunVelocity);


            modelBuilder.Entity<BodyEntity>().HasData(new
            {
                Id = 1,
                Name = "Sun",
                Mass = Constants.SolarMass,
                PositionId = sunPosition.Id,
                VelocityId = sunVelocity.Id,
                Radius = 0.0,
                Enabled = true
            });
        }
    }
}
