namespace Sas.Identity.Service.Models
{
    public class AuthenticateResponse
    {
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public AuthenticateResponse(string userName, string jwtToken)
        {
            Name = userName;
            JwtToken = jwtToken;
        }
    }
}
