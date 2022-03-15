using System.ComponentModel.DataAnnotations;

namespace Sas.Identity.Service.Models
{
    public class UserEntity : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public virtual List<RoleEntity> Roles { get; set;}
    }
}
