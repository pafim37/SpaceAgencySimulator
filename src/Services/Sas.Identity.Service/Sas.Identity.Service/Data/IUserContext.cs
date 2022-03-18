using Sas.Identity.Service.Models.Entities;
using System.Data.Entity;

namespace Sas.Identity.Service.Data
{
    public interface IUserContext
    {
        DbSet<UserEntity> Users { get; set; }
        DbSet<RoleEntity> Roles { get; set; }
    }
}
