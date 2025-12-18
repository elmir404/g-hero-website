using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    /// <summary>
    /// Submit a contact form
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SubmitContact([FromBody] ContactRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }

        try
        {
            _logger.LogInformation("Contact form submission received from: {Email}", request.Email);
            var contactId = await _contactService.CreateContactAsync(request);
            _logger.LogInformation("Contact form submitted successfully. ID: {ContactId}", contactId);
            return Ok(new { 
                success = true, 
                message = "Thank you for your message! We will get back to you soon.",
                id = contactId 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting contact form from: {Email}", request.Email);
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while submitting your message. Please try again later." 
            });
        }
    }
}

