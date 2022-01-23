using Sas.Db.ObservationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.ObservationDatabase.Data
{
    public class ObservationDb : DbContext
    {
        public DbSet<Observatory> Observatories { get; set; }
        public DbSet<Observation> Observations { get; set; }

        public ObservationDb() : base()
        {
            Database.SetInitializer(new ObservationDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Observatory>().HasMany<Observation>().WithRequired(s => s.ObservatoryId).HasForeignKey<Observatory>(s => s.ObservatoryId);
            modelBuilder.Entity<Observation>().HasRequired(x => x.Observatory).WithMany(x => x.Observations).HasForeignKey(x => x.ObservatoryId);
        }
    }
}
