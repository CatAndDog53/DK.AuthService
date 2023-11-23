using DK.AuthService.Model.Dtos;
using DK.AuthService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DK.AuthService.Services.Interfaces;

namespace DK.AuthService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;

        public UserController(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        [Route("GetCurrentUserInfo")]
        [Authorize(Roles = PredefinedUserRoles.USER)]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var userInfo = await _userService.GetCurrentUserInfo(User.Identity?.Name);

            return Ok(userInfo);
        }

        [HttpGet]
        [Route("GetAllUsersInfo")]
        [Authorize(Roles = PredefinedUserRoles.ADMIN)]
        public async Task<IActionResult> GetAllUsersInfo()
        {
            var userInfo = await _userService.GetAllUsersInfo();

            return Ok(userInfo);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDataDto registerDto)
        {
            var registerResult = await _userService.RegisterAsync(registerDto);

            if (registerResult.IsSucceed)
                return Ok(registerResult);

            return BadRequest(registerResult);
        }

        [HttpPost]
        [Route("updateUserInfo")]
        [Authorize(Roles = PredefinedUserRoles.USER)]
        public async Task<IActionResult> UpdateCurrentUserInfo([FromBody] UpdateUserInfoDto updateUserInfoDto)
        {
            var updateResult = await _userService.UpdateUserInfoAsync(User.Identity?.Name, updateUserInfoDto);

            if (updateResult.IsSucceed)
                return Ok(updateResult);

            return BadRequest(updateResult);
        }

        [HttpPost]
        [Route("changeUserPassword")]
        [Authorize(Roles = PredefinedUserRoles.USER)]
        public async Task<IActionResult> ChangeCurrentUserPassword([FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            var passwordChangeResult = await _userService.ChangeUserPassword(User.Identity?.Name, changeUserPasswordDto);

            if (passwordChangeResult.IsSucceed)
                return Ok(passwordChangeResult);

            return BadRequest(passwordChangeResult);
        }

        [HttpPost]
        [Route("addAdminRoleToUser")]
        [Authorize(Roles = PredefinedUserRoles.ADMIN)]
        public async Task<IActionResult> AddAdminRoleToUser([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _userService.AddAdminRoleToUserAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }
    }
}
