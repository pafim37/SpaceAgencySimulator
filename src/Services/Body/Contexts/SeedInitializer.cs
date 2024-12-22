﻿using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Contexts
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
            
            BodyEntity sunBody = new()
            {
                Id = 1,
                Name = "Sun",
                Mass = 100,
                PositionId = sunPosition.Id,
                VelocityId = sunVelocity.Id,
                Radius = 0
            };

            VectorEntity earthPosition = new()
            {
                Id = 3,
                X = 1,
                Y = 0,
                Z = 0,
            };

            VectorEntity earthVelocity = new()
            {
                Id = 4,
                X = 0,
                Y = 1,
                Z = 0,
            };

            BodyEntity earthBody = new()
            {
                Id = 2,
                Name = "Earth",
                Mass = 1,
                PositionId = earthPosition.Id,
                VelocityId = earthVelocity.Id,
                Radius = 0
            };

            modelBuilder.Entity<VectorEntity>().HasData(sunPosition);
            modelBuilder.Entity<VectorEntity>().HasData(sunVelocity);

            modelBuilder.Entity<VectorEntity>().HasData(earthPosition);
            modelBuilder.Entity<VectorEntity>().HasData(earthVelocity);

            modelBuilder.Entity<BodyEntity>().HasData(sunBody);
            modelBuilder.Entity<BodyEntity>().HasData(earthBody);
        }
    }
}
