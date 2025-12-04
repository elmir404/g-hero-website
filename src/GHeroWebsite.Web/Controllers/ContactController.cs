using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.Web.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([FromForm] ContactDto model)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["Error"] = $"Please correct the errors: {errors}";
            return RedirectToAction("Index", "Home", new { anchor = "contact" });
        }

        try
        {
            await _contactService.CreateContactAsync(model);
            TempData["Success"] = "Thank you for your message! We will get back to you soon.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting contact form");
            TempData["Error"] = "An error occurred while submitting your message. Please try again later.";
        }

        return RedirectToAction("Index", "Home", new { anchor = "contact" });
    }
}

