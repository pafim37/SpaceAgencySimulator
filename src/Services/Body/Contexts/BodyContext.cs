using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Models;

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

            var position = new VectorEntity()
            {
                Id = 1,
                X = 0,
                Y = 0,
                Z = 0,
            };
            var velocity = new VectorEntity()
            {
                Id = 2,
                X = 0,
                Y = 0,
                Z = 0,
            };
            modelBuilder.Entity<VectorEntity>().HasData(position);
            modelBuilder.Entity<VectorEntity>().HasData(velocity);
            
            BodyEntity body = new()
            {
                Id = 1,
                Name = "Sun",
                Mass = 1,
                PositionId = position.Id,
                VelocityId = velocity.Id,
                Radius = 0
            };

            modelBuilder.Entity<BodyEntity>().HasData(body);
        }
    }
}
