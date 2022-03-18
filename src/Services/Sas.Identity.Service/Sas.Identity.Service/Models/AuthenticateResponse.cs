using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Models
{
    public class AuthenticateResponse
    {
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public AuthenticateResponse(UserEntity user, string jwtToken)
        {
            Name = user.Name;
            JwtToken = jwtToken;
        }
    }
}
