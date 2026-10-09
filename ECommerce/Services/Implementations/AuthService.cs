using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceOrderManagement.DTOs.Auth;
using ECommerceOrderManagement.Models;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceOrderManagement.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser =
                await _userManager.FindByNameAsync(dto.UserName);

            if (existingUser != null)
                throw new InvalidOperationException(
                    "Username already exists.");

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                CustomerId = dto.CustomerId
            };

            var result =
                await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            await _userManager.AddToRoleAsync(
                user,
                "Customer");

            return "User registered successfully.";
        }

        public async Task<LoginResponseDto> LoginAsync(
            LoginDto dto)
        {
            var user =
                await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
                throw new UnauthorizedAccessException(
                    "Invalid username or password.");

            var validPassword =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password);

            if (!validPassword)
                throw new UnauthorizedAccessException(
                    "Invalid username or password.");

            var roles =
                await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName ?? string.Empty),

                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty)
            };

            claims.AddRange(
                roles.Select(role =>
                    new Claim(ClaimTypes.Role, role)));

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var expiration =
                DateTime.UtcNow.AddMinutes(
                    double.Parse(
                        _configuration[
                            "Jwt:DurationInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new LoginResponseDto
            {
                Token =
                    new JwtSecurityTokenHandler()
                        .WriteToken(token),

                Expiration = expiration
            };
        }
    }
}