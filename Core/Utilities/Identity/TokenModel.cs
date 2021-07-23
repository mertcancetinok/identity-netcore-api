using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
namespace Core.Utilities.Identity
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Username { get; set; }
  
        public void CreateToken(List<Claim> authClaim,IConfiguration configuration)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(issuer: configuration["JWT:ValidIssuer"],
                                         audience: configuration["JWT:ValidAudience"],
                                         expires: DateTime.Now.AddHours(3),
                                         claims: authClaim,
                                         signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
             );
            
            Token = new JwtSecurityTokenHandler().WriteToken(token);
            Expiration = token.ValidTo;

        }

    }
  
}
