using Sas.Identity.Service.Models.Entities;
using System.Data.Entity;

namespace Sas.Identity.Service.Data
{
    public class UserContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }

        public UserContext() : base()
        {
            Database.SetInitializer(new UserContextSeed());
        }
    }
}
