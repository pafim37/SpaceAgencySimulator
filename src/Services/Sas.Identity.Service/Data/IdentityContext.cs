using Sas.Identity.Service.Entities;
using System.Data.Entity;

namespace Sas.Identity.Service.Data
{
    public class IdentityContext : DbContext, IIdentityContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public IdentityContext() : base()
        {
        }
    }
}
