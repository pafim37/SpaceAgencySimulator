using Sas.Astronomy.Service.Models;
using System.Data.Entity;

namespace Sas.Astronomy.Service.Data
{
    public class Context : DbContext
    {
        public DbSet<ObservatoryEntity> Observatories { get; set; }
        public DbSet<ObservationEntity> Observations { get; set; }

        public Context() : base()
        {
            Database.SetInitializer(new ContextSeed());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObservationEntity>().HasRequired(x => x.Observatory).WithMany(x => x.Observations).HasForeignKey(x => x.ObservatoryId);
        }
    }
}
