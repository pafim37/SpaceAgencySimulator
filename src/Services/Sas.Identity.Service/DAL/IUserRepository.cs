using Sas.Identity.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetUsers(); 
    }
}
