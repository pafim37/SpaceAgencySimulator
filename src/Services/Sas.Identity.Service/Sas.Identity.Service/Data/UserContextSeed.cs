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
                SaltPre = "WkYol8",
                SaltPost = "der22Q",
                PasswordHash = BCryptNet.HashPassword("WkYol8" + "admin" + "der22Q"),
                Roles = new List<RoleEntity>(){ new RoleEntity() { Role = Role.Admin } }
            };

            UserEntity goodUser = new UserEntity() {
                Name = "test2",
                SaltPre = "der22Q",
                SaltPost = "WkYol8",
                PasswordHash = BCryptNet.HashPassword("der22Q" + "good" + "WkYol8"),
                Roles = new List<RoleEntity>(){ new RoleEntity() { Role = Role.God } }
            };

            UserEntity normalUser = new UserEntity() {
                Name = "test3",
                SaltPre = "der22Q",
                SaltPost = "WkYol8",
                PasswordHash = BCryptNet.HashPassword("der22Q" + "normal" + "WkYol8"),
                Roles = new List<RoleEntity>() { new RoleEntity() { Role = Role.Astronomer } } 
            };
            
            UserEntity allRoleUser = new UserEntity() {
                Name = "test4",
                SaltPre = "der22Q",
                SaltPost = "WkYol8",
                PasswordHash = BCryptNet.HashPassword("der22Q" + "all" + "WkYol8"),
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
