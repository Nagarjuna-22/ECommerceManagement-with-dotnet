using ECommerceOrderManagement.DTOs.Auth;
using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.Controllers
{
    [ApiController]
    [Route("api/auth/[Action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);

            var response = new ApiResponse<string>
            {
                Status = true,
                Message = "User registered successfully.",
                Data = result
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);

            var response = new ApiResponse<LoginResponseDto>
            {
                Status = true,
                Message = "Login successful.",
                Data = result
            };

            return Ok(response);
        }
    }
}