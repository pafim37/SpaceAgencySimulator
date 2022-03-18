using Sas.Domain.Users;
using Sas.Identity.Service.Models;
using Sas.Identity.Service.Models.Entities;

namespace Sas.Identity.Service.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        User GetById(int id);
        Task<User> GetByNameAsync(string name);
        Task<IEnumerable<User>> GetAll();
        Task CreateAsync(UserEntity user);
    }
}
