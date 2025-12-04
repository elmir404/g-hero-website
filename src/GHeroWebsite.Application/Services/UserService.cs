using GHeroWebsite.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GHeroWebsite.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> CreateUserAsync(string username, string email, string password, string? fullName = null)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return false;

        var user = new ApplicationUser
        {
            UserName = username,
            Email = email,
            FullName = fullName,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true // For admin users, we can auto-confirm
        };

        var result = await _userManager.CreateAsync(user, password);
        
        if (result.Succeeded)
        {
            // Add to Admin role
            await _userManager.AddToRoleAsync(user, "Admin");
            return true;
        }

        return false;
    }

    public async Task UpdateLastLoginAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }
    }
}
