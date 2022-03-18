using AutoMapper;
using Sas.Domain.Users;
using Sas.Identity.Service.Data;
using Sas.Identity.Service.Models;
using Sas.Identity.Service.Models.Entities;
using Sas.Identity.Service.Utils;
using System.Data.Entity;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sas.Identity.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(UserContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == model.Name);

            var password = model.Password + user.Salt;

            if (user == null || !BCryptNet.Verify(password, user.PasswordHash.ToString()))
                throw new Exception();

            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user.Name, jwtToken);
        }

        public User GetById(int id)
        {
            var userEntity = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            return _mapper.Map<User>(userEntity);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var userEntities = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<User>>(userEntities);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            var userEntity = await _context.Users.Where(x => x.Name == name).FirstOrDefaultAsync();
            return _mapper.Map<User>(userEntity);
        }

        public async Task CreateAsync(UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
