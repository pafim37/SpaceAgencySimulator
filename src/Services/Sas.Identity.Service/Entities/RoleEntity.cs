using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Entities
{
    public class RoleEntity : EntityBase
    {
        public Role RoleType { get; set; }

        public int UserId { get; set; }
    }
}
