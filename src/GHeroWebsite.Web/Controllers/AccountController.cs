using GHeroWebsite.Application.Services;
using GHeroWebsite.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IUserService userService,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null, bool rememberMe = false)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Username and password are required.");
            return View();
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(
            username, 
            password, 
            rememberMe, 
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            // Update last login
            await _userService.UpdateLastLoginAsync(user.Id);
            
            _logger.LogInformation("User {Username} logged in", username);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "" });
        }
        
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User {Username} account locked out", username);
            ModelState.AddModelError("", "Your account has been locked out. Please try again later.");
            return View();
        }

        ModelState.AddModelError("", "Invalid username or password.");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
