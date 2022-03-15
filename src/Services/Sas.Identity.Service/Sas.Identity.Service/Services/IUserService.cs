using Sas.Identity.Service.Models;

namespace Sas.Identity.Service.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        Task<UserEntity> GetById(int id);
        Task<UserEntity> GetByNameAsync(string name);
        Task<IEnumerable<UserEntity>> GetAll();
        Task CreateAsync(UserEntity user);
    }
}
