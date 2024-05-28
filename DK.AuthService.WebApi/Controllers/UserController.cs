using DK.AuthService.Model.Dtos;
using DK.AuthService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DK.AuthService.Services.Interfaces;
using DK.AuthService.WebApi.AuthorizationAttributes;

namespace DK.AuthService.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("user/{username}")]
        [ResourceOwnerOrAdmin]
        public async Task<IActionResult> GetUserInfo(string? username)
        {            
            UserInfoDto userInfo;
            try
            {
                userInfo = await _userService.GetUserInfo(username);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(userInfo);
        }

        [HttpGet]
        [Route("users")]
        [Authorize(Roles = PredefinedUserRoles.ADMIN)]
        public async Task<IActionResult> GetAllUsersInfo()
        {
            var userInfo = await _userService.GetAllUsersInfo();

            return Ok(userInfo);
        }

        [HttpPost]
        [Route("user/{username}/update")]
        [ResourceOwnerOrAdmin]
        public async Task<IActionResult> UpdateUserInfo(string? username, [FromBody] UpdateUserInfoDto updateUserInfoDto)
        {
            try
            {
                await _userService.UpdateUserInfoAsync(username, updateUserInfoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("User info updated successfully!");
        }

        [HttpPost]
        [Route("user/{username}/changePassword")]
        [ResourceOwnerOrAdmin]
        public async Task<IActionResult> ChangeUserPassword(string? username, [FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            try
            {
                await _userService.ChangeUserPassword(username, changeUserPasswordDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Password changed successfully!");
        }

        [HttpPost]
        [Route("user/{username}/addToRole")]
        [Authorize(Roles = PredefinedUserRoles.ADMIN)]
        public async Task<IActionResult> AddToRole(string? username, string roleName)
        {
            try
            {
                await _userService.AddToRoleAsync(username, roleName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("User role updated successfully!");
        }

        [HttpPost]
        [Route("user/{username}/removeFromRole")]
        [Authorize(Roles = PredefinedUserRoles.ADMIN)]
        public async Task<IActionResult> RemoveFromRole(string? username, string roleName)
        {
            try
            {
                await _userService.RemoveFromRoleAsync(username, roleName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("User role updated successfully!");
        }
    }
}
