using Sas.Domain.Users;

namespace Sas.Identity.Service.Models.Entities
{
    public class RoleEntity : Entity
    {
        public Role Role { get; set; }
        public virtual List<UserEntity> Users { get; set; }
    }
}
