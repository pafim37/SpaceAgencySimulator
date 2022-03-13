﻿using Sas.Identity.Service.Models;
using System.Data.Entity;
using BCryptNet = BCrypt.Net.BCrypt;
namespace Sas.Identity.Service.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext() : base()
        {
            Database.SetInitializer(new UserContextSeed());
        }
    }
}
