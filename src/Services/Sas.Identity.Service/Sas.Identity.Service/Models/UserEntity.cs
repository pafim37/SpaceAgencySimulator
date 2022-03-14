namespace Sas.Identity.Service.Models
{
    public class UserEntity : Entity
    {
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string SaltPre { get; set; }
        public string SaltPost { get; set; }
        public virtual List<RoleEntity> Roles { get; set;}
    }
}
