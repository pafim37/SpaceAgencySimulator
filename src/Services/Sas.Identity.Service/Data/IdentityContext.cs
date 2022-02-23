using Microsoft.AspNetCore.Identity;
using Sas.Identity.Service.Entities;
using System.Data.Entity;

namespace Sas.Identity.Service.Data
{
    public class IdentityContext : DbContext, IIdentityContext
    {
        public DbSet<IdentityUser> Users { get; set; }

        public IdentityContext() : base()
        {
        }
    }
}
