using Microsoft.Extensions.Options;
using Sas.Identity.Service.Autorizations;
using Sas.Identity.Service.Config;
using Sas.Identity.Service.Data;
using Sas.Identity.Service.Models;
using System.Data.Entity;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sas.Identity.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly IJwtUtils _jwtUtils;
        private readonly Settings _settings;

        public UserService(UserContext context, IJwtUtils jwtUtils, IOptions<Settings> settings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _settings = settings.Value;
        }
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            var user =  await _context.Users.SingleOrDefaultAsync(x => x.Name == model.Name);

            var password = model.Password + user.Salt;

            if (user == null || !BCryptNet.Verify(password, user.PasswordHash.ToString()))
                throw new Exception();

            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken);
        }

        public UserEntity GetById(int id)
        {
            return _context.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> GetByNameAsync(string name)
        {
            return await _context.Users.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
