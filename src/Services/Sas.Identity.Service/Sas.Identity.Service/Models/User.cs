using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
