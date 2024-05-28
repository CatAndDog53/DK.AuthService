using Microsoft.AspNetCore.Mvc;
using DK.AuthService.Services.Interfaces;
using DK.AuthService.Model.Dtos;

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
            LoginResponseDto loginResponseDto;
            try
            {
                loginResponseDto = await _authService.LoginAsync(loginDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(loginResponseDto);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            try
            {
                await _authService.RegisterAsync(registerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Registration successfull!");
        }
    }
}
