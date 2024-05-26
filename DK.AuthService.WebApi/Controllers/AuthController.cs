using Microsoft.AspNetCore.Mvc;
using DK.AuthService.Services.Interfaces;
using DK.AuthService.Model.Dtos;
using DK.AuthService.Model;
using Microsoft.AspNetCore.Authorization;

namespace DK.AuthService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);

            if (loginResult.IsSucceed)
                return Ok(loginResult);

            return Unauthorized(loginResult);
        }
        
        [HttpPost]
        [Route("externalLogin")]
        [Authorize(Roles = PredefinedUserRoles.USER)]
        public async Task<IActionResult> ExternalLogin()
        {
            var externalToken = await _authService.GetExternalTokenAsync(User.Identity?.Name);

            if (externalToken.IsSucceed)
                return Ok(externalToken);

            return Unauthorized(externalToken);
        }

    }
}
