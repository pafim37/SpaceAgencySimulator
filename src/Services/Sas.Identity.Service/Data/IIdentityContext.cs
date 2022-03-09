using Sas.Identity.Service.Entities;
using System.Data.Entity;

namespace Sas.Identity.Service.Data
{
    public interface IIdentityContext
    {
        DbSet<UserEntity> Users { get; set; }
    }
}
