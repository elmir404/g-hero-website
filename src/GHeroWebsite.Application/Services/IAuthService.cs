using GHeroWebsite.Application.DTOs;

namespace GHeroWebsite.Application.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<bool> ValidateTokenAsync(string token);
}

