using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace Sas.Identity.Service.Data
{
    public interface IIdentityContext
    {
        DbSet<IdentityUser> Users { get; set; }
    }
}
