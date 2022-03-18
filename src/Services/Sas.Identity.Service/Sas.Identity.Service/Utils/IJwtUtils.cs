using Sas.Identity.Service.Models.Entities;

namespace Sas.Identity.Service.Utils
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(UserEntity user);
        public int? ValidateJwtToken(string token);
    }
}
