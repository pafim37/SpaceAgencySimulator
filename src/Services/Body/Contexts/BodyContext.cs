using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Contexts
{
    public class BodyContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<BodyEntity> Bodies { get; set; }
        public DbSet<VectorEntity> Vectors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BodyEntity>()
                .HasOne(b => b.Position)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<BodyEntity>()
                .HasOne(b => b.Velocity)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BodyEntity>()
                .Navigation(b => b.Position)
                .AutoInclude();

            modelBuilder.Entity<BodyEntity>()
                .Navigation(b => b.Velocity)
                .AutoInclude();

            SeedInitializer.Seed(modelBuilder);
        }
    }
}
