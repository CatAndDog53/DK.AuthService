using DK.AuthService.Model.Dtos;

namespace DK.AuthService.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoDto> GetUserInfo(string? currentUserName);
        Task<IEnumerable<UserInfoDto>> GetAllUsersInfo();
        Task<bool> AddToRoleAsync(string? username, string? roleName);
        Task<bool> RemoveFromRoleAsync(string? username, string? roleName);
        Task<bool> UpdateUserInfoAsync(string? currentUserName, UpdateUserInfoDto updateUserInfoDto);
        Task<bool> ChangeUserPassword(string? currentUserName, ChangeUserPasswordDto changeUserPasswordDto);
    }
}
