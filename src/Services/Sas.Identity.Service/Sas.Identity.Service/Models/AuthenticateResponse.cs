using Sas.Identity.Service.Models.Entities;

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
