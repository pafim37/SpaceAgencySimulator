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
        public int Id { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public AuthenticateResponse(UserEntity user, string jwtToken)
        {
            Id = user.Id;
            Name = user.Name;
            JwtToken = jwtToken;
        }
    }
}
