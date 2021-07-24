using Business.Abstract;
using Business.Constants;
using Core.Utilities.Identity;
using Core.Utilities.Results;
using Entities.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Business.Utilities.Identity;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration Configuration;
        public AuthManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            Configuration = configuration;  
        }
        public async Task<IResult> Register(RegisterDto registerDto)
        {
            var userExist = await userManager.FindByNameAsync(registerDto.UserName);
            if (userExist != null)
            {
                return new ErrorResult(Messages.UserExists);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.UserName
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    return new ErrorResult(error.Description);
                }
                
            }

            return new SuccessResult(Messages.UserSuccess);
        }
        public async Task<IDataResult<TokenModel>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
                return new ErrorDataResult<TokenModel>(Messages.UserNotFound);
            if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
                return new ErrorDataResult<TokenModel>(Messages.PasswordIncorrect);

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var tokenModel  = new TokenModel();
            tokenModel.CreateToken(authClaims, Configuration);
            tokenModel.Username = loginDto.Username;
            return new SuccessDataResult<TokenModel>(tokenModel,Messages.LoginSuccess);
        }

        public async Task<IResult> RegisterAdmin(RegisterDto registerDto)
        {
            var userExist = await userManager.FindByNameAsync(registerDto.UserName);
            if (userExist != null)
            {
                return new ErrorResult(Messages.UserExists);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.UserName
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    return new ErrorResult(error.Description);
                }

            }

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if(await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            return new SuccessResult(Messages.UserSuccess);
        }
    }
}
