using ECommerceOrderManagement.DTOs.Auth;

namespace ECommerceOrderManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);

        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}