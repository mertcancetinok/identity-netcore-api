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

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        public AuthManager(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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
    }
}
