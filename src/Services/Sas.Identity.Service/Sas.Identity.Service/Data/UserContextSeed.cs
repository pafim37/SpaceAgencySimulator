using Sas.Domain.Users;
using Sas.Identity.Service.Models;
using System.Data.Entity;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sas.Identity.Service.Data
{
    public class UserContextSeed : CreateDatabaseIfNotExists<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            UserEntity adminUser = new UserEntity() {
                Name = "test1",
                Salt = "WkYol8",
                PasswordHash = BCryptNet.HashPassword("admin" + "WkYol8"),
                Roles = new List<RoleEntity>(){ new RoleEntity() { Role = Role.Admin } }
            };

            UserEntity goodUser = new UserEntity() {
                Name = "test2",
                Salt = "der22Q",
                PasswordHash = BCryptNet.HashPassword("good" + "der22Q"),
                Roles = new List<RoleEntity>(){ new RoleEntity() { Role = Role.God } }
            };

            UserEntity normalUser = new UserEntity() {
                Name = "test3",
                Salt = "rt4wer",
                PasswordHash = BCryptNet.HashPassword("normal" + "rt4wer"),
                Roles = new List<RoleEntity>() { new RoleEntity() { Role = Role.Astronomer } } 
            };
            
            UserEntity allRoleUser = new UserEntity() {
                Name = "test4",
                Salt = "ytre34",
                PasswordHash = BCryptNet.HashPassword("all" + "ytre34"),
                Roles = new List<RoleEntity>() { new RoleEntity() { Role = Role.Admin }, new RoleEntity() { Role = Role.God }, new RoleEntity() { Role = Role.Astronomer } } 
            };

            context.Users.Add(adminUser);
            context.Users.Add(goodUser);
            context.Users.Add(normalUser);
            context.Users.Add(allRoleUser);

            base.Seed(context);
        }
    }
}
