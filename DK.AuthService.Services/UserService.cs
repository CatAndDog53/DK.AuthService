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

        public async Task<UserInfoDto> GetCurrentUserInfo(string? currentUserName)
        {
            if (!await IsCurrentUserNameValid(currentUserName))
            {
                return new UserInfoDto();
            }

            var currentUser = await _userManager.FindByNameAsync(currentUserName);

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

        public async Task<ServiceResponseDto> RegisterAsync(RegisterDataDto registerDto)
        {
            var userByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            var userByName = await _userManager.FindByNameAsync(registerDto.UserName);

            if (userByEmail != null)
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = $"User with email {userByEmail.Email} already exists!"
                };

            if (userByName != null)
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = $"User with username {userByName.UserName} already exists!"
                };


            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User creation failed beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            await _userManager.AddToRoleAsync(newUser, PredefinedUserRoles.USER);

            return new ServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User created successfully!"
            };
        }

        public async Task<ServiceResponseDto> AddAdminRoleToUserAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByEmailAsync(updatePermissionDto.Email);

            if (user is null)
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Email!"
                };

            await _userManager.AddToRoleAsync(user, PredefinedUserRoles.ADMIN);

            return new ServiceResponseDto()
            {
                IsSucceed = true,
                Message = $"User with email {user.Email} is now an ADMIN"
            };
        }

        public async Task<ServiceResponseDto> UpdateUserInfoAsync(string? currentUserName, UpdateUserInfoDto updatedUserInfo)
        {
            var userWithWantedUsername = await _userManager.FindByNameAsync(updatedUserInfo.UserName);
            if (userWithWantedUsername != null)
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = $"Username {userWithWantedUsername.UserName} is already taken!"
                };

            if (!await IsCurrentUserNameValid(currentUserName))
            {
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Access denied"
                };
            }

            var userWithCurrentUsername = await _userManager.FindByNameAsync(currentUserName);

            userWithCurrentUsername.FirstName = updatedUserInfo.FirstName;
            userWithCurrentUsername.LastName = updatedUserInfo.LastName;
            userWithCurrentUsername.UserName = updatedUserInfo.UserName;

            var updateUserResult = await _userManager.UpdateAsync(userWithCurrentUsername);

            if (!updateUserResult.Succeeded)
            {
                var errorString = "User update failed beacause: ";
                foreach (var error in updateUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            return new ServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User updated successfully!"
            };
        }

        public async Task<ServiceResponseDto> ChangeUserPassword(string? currentUserName, ChangeUserPasswordDto changeUserPasswordDto)
        {
            if (!await IsCurrentUserNameValid(currentUserName))
            {
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Access denied"
                };
            }

            var userWithCurrentUsername = await _userManager.FindByNameAsync(currentUserName);

            var passwordChangeResult = await _userManager.ChangePasswordAsync(
                userWithCurrentUsername,
                changeUserPasswordDto.OldPassword,
                changeUserPasswordDto.NewPassword);

            if (!passwordChangeResult.Succeeded)
            {
                var errorString = "Password change failed beacause: ";
                foreach (var error in passwordChangeResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new ServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            return new ServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Password was successfully changed!"
            };
        }

        public async Task<bool> IsCurrentUserNameValid(string? currentUserName)
        {
            if (currentUserName == null)
            {
                return false;
            }

            var userWithCurrentUsername = await _userManager.FindByNameAsync(currentUserName);
            if (userWithCurrentUsername == null)
            {
                return false;
            }
            return true;
        }
    }
}
