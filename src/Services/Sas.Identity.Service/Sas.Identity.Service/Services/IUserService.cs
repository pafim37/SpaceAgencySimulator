using Sas.Identity.Service.Models;

namespace Sas.Identity.Service.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        UserEntity GetById(int id);
        Task<IEnumerable<UserEntity>> GetAll();
    }
}
