using DK.AuthService.Model.Dtos;

namespace DK.AuthService.Services.Interfaces
{
    public interface IAuthenticationService: IDisposable
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> RegisterAsync(RegisterRequestDto registerRequestDto);
    }
}
