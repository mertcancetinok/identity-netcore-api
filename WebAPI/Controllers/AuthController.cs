using Business.Abstract;
using Core.Utilities.Identity;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto model)
        {
            var result = await _authService.Register(model);
            return result.Success ? Ok(result) : BadRequest(result);
            
        }
        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterAdmin(model);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.Login(model);
            return result.Success ? Ok(result) : Unauthorized();

        }
    }
}
