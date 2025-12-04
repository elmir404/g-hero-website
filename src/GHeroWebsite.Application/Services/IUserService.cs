using GHeroWebsite.Domain.Entities;

namespace GHeroWebsite.Application.Services;

public interface IUserService
{
    Task<ApplicationUser?> GetUserByIdAsync(string id);
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<ApplicationUser?> GetUserByUsernameAsync(string username);
    Task<bool> CreateUserAsync(string username, string email, string password, string? fullName = null);
    Task UpdateLastLoginAsync(string userId);
}
