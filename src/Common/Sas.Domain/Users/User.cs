namespace Sas.Domain.Users
{
    public class User
    {
        public string Name { get; set; }
        public List<Role> Roles { get; set; }
    }
}
