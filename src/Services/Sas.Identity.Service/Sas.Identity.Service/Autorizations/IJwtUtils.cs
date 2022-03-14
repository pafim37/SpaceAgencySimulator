using Sas.Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Autorizations
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(UserEntity user);
        public int? ValidateJwtToken(string token);
    }
}
