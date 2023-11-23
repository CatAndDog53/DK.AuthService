using DK.AuthService.Model.Dtos;

namespace DK.AuthService.Services.Interfaces
{
    public interface IAuthenticationService: IDisposable
    {
        Task<ServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<ServiceResponseDto> GetExternalTokenAsync(string? userName);
    }
}
