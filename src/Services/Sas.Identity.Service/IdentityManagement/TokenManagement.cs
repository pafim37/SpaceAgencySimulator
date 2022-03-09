using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.IdentityManagement
{
    public class TokenManagement
    {
        //private JwtSecurityTokenHandler _tokenHandler;
        //private byte[] _secretKey;

        //public TokenManagement()
        //{
        //    _tokenHandler = new JwtSecurityTokenHandler();
        //    _secretKey = Encoding.UTF8.GetBytes("q#we1rty2u$io3plk4jhg5fds6a&z&x7cvb8n!m9@0");
        //}
        //public bool IsAuthenticated(string username, string password)
        //{
        //    return true;
        //}

        //public async Task<dynamic> GenerateToken(string username)
        //{
        //    // var user = await _context.Set<UserEntity>().Where(u => u.UserName.Equals(username)).FirstAsync();
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, username),
        //    };

        //    var token = new JwtSecurityToken(
        //        new JwtHeader(
        //            new SigningCredentials(
        //                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("q#we1rty2u$io3plk4jhg5fds6a&z&x7cvb8n!m9@0")),
        //                SecurityAlgorithms.HmacSha256)),
        //        new JwtPayload(claims));

        //    var output = new
        //    {
        //        Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
        //        UserName = username
        //    };

        //    return output;
        //}

        //public bool VerifyToken(string token)
        //{
        //    var claims = _tokenHandler.ValidateToken(token, 
        //        new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = false,
        //            IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ValidateLifetime = false
        //        }, out SecurityToken validatedToken);
        //    return claims;
        //}
    }
}
