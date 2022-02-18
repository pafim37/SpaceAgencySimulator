using Sas.Astronomy.Service.Models;
using System.Data.Entity;

namespace Sas.Astronomy.Service.Data
{
    public class AstronomyContext : DbContext
    {
        public DbSet<ObservatoryEntity> Observatories { get; set; }
        public DbSet<ObservationEntity> Observations { get; set; }

        public AstronomyContext() : base()
        {
            Database.SetInitializer(new AstronomyContextSeed());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObservationEntity>().HasRequired(x => x.Observatory).WithMany(x => x.Observations).HasForeignKey(x => x.ObservatoryId);
        }
    }
}
