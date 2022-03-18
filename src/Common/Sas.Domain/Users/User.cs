namespace Sas.Domain.Users
{
    public class User
    {
        /// <summary>
        /// Nick name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of user roles
        /// </summary>
        public List<Role> Roles { get; set; }
    }
}
