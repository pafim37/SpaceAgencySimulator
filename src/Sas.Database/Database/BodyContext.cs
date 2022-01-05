using Microsoft.EntityFrameworkCore;
using Sas.BodyDatabase.Entities;

namespace Sas.BodyDatabase.Database
{
    public class BodyContext : DbContext
    {
        private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BodyContext;Trusted_Connection=True;";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DbSet<BodyEntity> Bodies { get; set; }
        public DbSet<PositionEntity> Positions { get; set; }
        public DbSet<VelocityEntity> Velocities { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BodyEntity>().HasOne(x => x.Position);
            modelBuilder.Entity<BodyEntity>().HasOne(x => x.Velocity);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            BodyEntity body1 = new() { Id = 1, Name = "body1", Mass = 1000, PositionId = 1, VelocityId = 1, Radius = 5 };
            BodyEntity body2 = new() { Id = 2, Name = "body2", Mass = 1000, PositionId = 2, VelocityId = 2, Radius = 5 };

            PositionEntity position1 = new() { Id = 1, X = 10, Y = 0, Z = 0 };
            PositionEntity position2 = new() { Id = 2, X = -10, Y = 0, Z = 0 };

            VelocityEntity velocity1 = new() { Id = 1, X = 0, Y = 10, Z = 0 };
            VelocityEntity velocity2 = new() { Id = 2, X = 0, Y = -10, Z = 0 };

            modelBuilder.Entity<BodyEntity>().HasData(body1);
            modelBuilder.Entity<BodyEntity>().HasData(body2);

            modelBuilder.Entity<PositionEntity>().HasData(position1);
            modelBuilder.Entity<PositionEntity>().HasData(position2);

            modelBuilder.Entity<VelocityEntity>().HasData(velocity1);
            modelBuilder.Entity<VelocityEntity>().HasData(velocity2);


            // BodyEntity Sun = new() { Id = 1, Name = "Sun", Mass = Constants.SolarMass, Radius = Constants.SunRadius, Position  = new PositionEntity(0, 0, 0), new Vector(0, 0, 0));

            //    Body Earth = new Body("Earth", Constants.EarthMass, new Vector(Constants.EarthApoapsis, 0, 0), new Vector(0, Constants.EarthMinVelocity, 0));

            //    Body Moon = new Body("Moon", Constants.MoonMass, new Vector(0, Constants.EarthApoapsis - Constants.MoonPeriapsis, 0), new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0));

            //    Body Probe = new Body("Probe", 100, new Vector(Constants.EarthPeriapsis - Constants.MoonPeriapsis - 400, 0, 0), new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0));

            //}
        }
    }
}
