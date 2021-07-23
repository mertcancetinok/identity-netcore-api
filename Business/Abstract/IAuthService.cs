using Core.Utilities.Identity;
using Core.Utilities.Results;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IResult> Register(RegisterDto registerDto);
        Task<IResult> RegisterAdmin(RegisterDto registerDto);
        Task<IDataResult<TokenModel>> Login(LoginDto loginDto);
    }
}
