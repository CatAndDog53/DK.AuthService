﻿using DK.AuthService.Model.Dtos;

namespace DK.AuthService.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoDto> GetCurrentUserInfo(string? currentUserName);
        Task<IEnumerable<UserInfoDto>> GetAllUsersInfo();
        Task<ServiceResponseDto> RegisterAsync(RegisterRequestDto registerDto);
        Task<ServiceResponseDto> UpdateRole(UpdateRoleRequestDto updatePermissionDto);
        Task<ServiceResponseDto> UpdateUserInfoAsync(string? currentUserName, UpdateUserInfoDto updateUserInfoDto);
        Task<ServiceResponseDto> ChangeUserPassword(string? currentUserName, ChangeUserPasswordDto changeUserPasswordDto);
        Task<bool> IsCurrentUserNameValid(string? currentUserName);
    }
}
