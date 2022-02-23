using Sas.Identity.Service.Data;
using Sas.Identity.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.DAL
{
    public class UserRepository 
    {

        private readonly IdentityContext _context;

        public UserRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return _context.Set<UserEntity>().ToList();
        }

        public async Task<bool> IsUserExist(UserEntity user)
        {
            return _context.Set<UserEntity>().Where(u => u.UserName == user.UserName).Any();
        }
    }
}
