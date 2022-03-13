using Sas.Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
