using Sas.Identity.Service.Models;
using System.Data.Entity;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sas.Identity.Service.Data
{
    public class UserContextSeed : CreateDatabaseIfNotExists<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            User testUser = new User() { Name = "pafim37", PasswordHash = BCryptNet.HashPassword("admin") };
            context.Users.Add(testUser);
            base.Seed(context);
        }
    }
}
