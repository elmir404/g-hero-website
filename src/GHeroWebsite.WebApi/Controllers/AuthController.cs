using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login and get JWT token
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }

        var result = await _authService.LoginAsync(request);
        if (result == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Get current user info (requires authentication)
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        try
        {
            _logger.LogInformation("GetCurrentUser called - IsAuthenticated: {IsAuth}, User: {User}", 
                User.Identity?.IsAuthenticated, User.Identity?.Name);
            
            var username = User.Identity?.Name;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation("User claims - Username: {Username}, Email: {Email}, Roles: {Roles}, UserId: {UserId}", 
                username, email, string.Join(", ", roles), userId);

            return Ok(new
            {
                username,
                email,
                roles,
                userId,
                isAuthenticated = User.Identity?.IsAuthenticated
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCurrentUser");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Test endpoint to check if authentication is working (for debugging)
    /// </summary>
    [HttpGet("test")]
    public IActionResult Test()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        
        return Ok(new
        {
            message = "Test endpoint - no auth required",
            hasAuthHeader = !string.IsNullOrEmpty(authHeader),
            authHeaderPreview = string.IsNullOrEmpty(authHeader) ? "None" : authHeader.Substring(0, Math.Min(30, authHeader.Length)),
            isAuthenticated,
            userName = User.Identity?.Name
        });
    }
}

