using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHomePageService _homePageService;

    public HomeController(IHomePageService homePageService)
    {
        _homePageService = homePageService;
    }

    /// <summary>
    /// Get complete home page data for all sections
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetHomePage([FromQuery] string lang = "en")
    {
        var homePage = await _homePageService.GetHomePageAsync(lang);
        return Ok(homePage);
    }

    /// <summary>
    /// Get hero section
    /// </summary>
    [HttpGet("hero")]
    public async Task<IActionResult> GetHero([FromQuery] string lang = "en")
    {
        var hero = await _homePageService.GetHeroSectionAsync(lang);
        if (hero == null)
            return NotFound(new { message = "Hero section not found" });
        
        return Ok(hero);
    }

    /// <summary>
    /// Get services list
    /// </summary>
    [HttpGet("services")]
    public async Task<IActionResult> GetServices([FromQuery] string lang = "en")
    {
        var services = await _homePageService.GetServicesAsync(lang);
        return Ok(services);
    }

    /// <summary>
    /// Get testimonials
    /// </summary>
    [HttpGet("testimonials")]
    public async Task<IActionResult> GetTestimonials([FromQuery] string lang = "en")
    {
        var testimonials = await _homePageService.GetTestimonialsAsync(lang);
        return Ok(testimonials);
    }

    /// <summary>
    /// Get FAQs
    /// </summary>
    [HttpGet("faqs")]
    public async Task<IActionResult> GetFaqs([FromQuery] string lang = "en")
    {
        var faqs = await _homePageService.GetFaqsAsync(lang);
        return Ok(faqs);
    }

    /// <summary>
    /// Get contact information
    /// </summary>
    [HttpGet("contact-info")]
    public async Task<IActionResult> GetContactInfo([FromQuery] string lang = "en")
    {
        var contactInfo = await _homePageService.GetContactInfoAsync(lang);
        if (contactInfo == null)
            return NotFound(new { message = "Contact info not found" });
        
        return Ok(contactInfo);
    }
}

