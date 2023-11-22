using DK.AuthService.Model.Dtos;

namespace DK.AuthService.Services.Interfaces
{
    public interface IAuthenticationService: IDisposable
    {
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDataDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginCredsDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
    }
}
