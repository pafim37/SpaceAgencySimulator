using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Models
{
    public class RoleEntity : Entity
    {
        public Role Role { get; set;}
        public virtual List<UserEntity> Users { get; set; }
    }
}
