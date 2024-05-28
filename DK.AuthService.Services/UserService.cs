using DK.AuthService.Model;
using DK.AuthService.Model.Dtos;
using DK.AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DK.AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserInfoDto> GetUserInfo(string? username)
        {
            var currentUser = await _userManager.FindByNameAsync(username);
            if (currentUser == null)
                throw new Exception("User not found!");

            return new UserInfoDto
            {
                UserName = currentUser.UserName,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email
            };
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersInfo()
        {
            return _userManager.Users.Select(user => new UserInfoDto { 
                UserName = user.UserName, 
                FirstName = user.FirstName, 
                LastName = user.LastName, 
                Email = user.Email 
            }).ToList();
        }
                
        public async Task<bool> AddToRoleAsync(string? username, string? roleName)
        {
            return await UpdateRoleAsync(username, roleName, true);
        }

        public async Task<bool> RemoveFromRoleAsync(string? username, string? roleName)
        {
            return await UpdateRoleAsync(username, roleName, false);
        }

        public async Task<bool> UpdateUserInfoAsync(string? currentUserName, UpdateUserInfoDto updatedUserInfo)
        {
            var userWithCurrentUsername = await _userManager.FindByNameAsync(currentUserName);
            if (userWithCurrentUsername == null)
                throw new Exception("User not found!");

            if (!String.IsNullOrEmpty(updatedUserInfo.UserName))
            {
                var userWithWantedUsername = await _userManager.FindByNameAsync(updatedUserInfo.UserName);
                if (userWithWantedUsername != null)
                    throw new ArgumentException($"Username {userWithWantedUsername.UserName} is already taken!");
            }

            if (!String.IsNullOrEmpty(updatedUserInfo.Email))
            {
                var userWithWantedEmail = await _userManager.FindByEmailAsync(updatedUserInfo.Email);
                if (userWithWantedEmail != null)
                    throw new ArgumentException($"Email {userWithWantedEmail.Email} is already taken!");
            }
                                             

            if (!String.IsNullOrEmpty(updatedUserInfo.FirstName)) userWithCurrentUsername.FirstName = updatedUserInfo.FirstName;
            if (!String.IsNullOrEmpty(updatedUserInfo.LastName)) userWithCurrentUsername.LastName = updatedUserInfo.LastName;
            if (!String.IsNullOrEmpty(updatedUserInfo.UserName)) userWithCurrentUsername.UserName = updatedUserInfo.UserName;
            if (!String.IsNullOrEmpty(updatedUserInfo.Email)) userWithCurrentUsername.Email = updatedUserInfo.Email;

            var updateUserResult = await _userManager.UpdateAsync(userWithCurrentUsername);

            if (!updateUserResult.Succeeded)
            {
                var errorString = string.Empty;
                foreach (var error in updateUserResult.Errors)
                    errorString += " # " + error.Description;
                
                throw new Exception(errorString);
            }

            return true;
        }

        public async Task<bool> ChangeUserPassword(string? currentUserName, ChangeUserPasswordDto changeUserPasswordDto)
        {
            var userWithCurrentUsername = await _userManager.FindByNameAsync(currentUserName);

            var passwordChangeResult = await _userManager.ChangePasswordAsync(
                userWithCurrentUsername,
                changeUserPasswordDto.OldPassword,
                changeUserPasswordDto.NewPassword);

            if (!passwordChangeResult.Succeeded)
            {
                var errorString = string.Empty;
                foreach (var error in passwordChangeResult.Errors)
                    errorString += " # " + error.Description;

                throw new Exception(errorString);
            }

            return true;
        }

        private async Task<bool> UpdateRoleAsync(string? username, string? roleName, bool addRole)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                throw new ArgumentException($"User {username} doesn't exist!");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new ArgumentException("Requested role doesn't exist!");

            if (await _userManager.IsInRoleAsync(user, roleName) && addRole)
                throw new ArgumentException("User already has requested role!");

            if (!await _userManager.IsInRoleAsync(user, roleName) && !addRole)
                throw new ArgumentException("User already isn't in requested role!");

            IdentityResult result;
            if (addRole)
                result = await _userManager.AddToRoleAsync(user, roleName);
            else
                result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
                return true;
            else
                return false;
        }
    }
}
