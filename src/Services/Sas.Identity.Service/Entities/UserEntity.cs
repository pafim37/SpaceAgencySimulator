using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Entities
{
    public class UserEntity : EntityBase
    {
        public SecureString UserName { get; set; }
        public SecureString Password { get; set; }
        public List<Role> Roles { get; set; }
    }
}
