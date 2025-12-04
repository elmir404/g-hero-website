using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.Web.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class DashboardController : Controller
{
    private readonly IContactService _contactService;

    public DashboardController(IContactService contactService)
    {
        _contactService = contactService;
    }

    public async Task<IActionResult> Index()
    {
        var contacts = await _contactService.GetAllContactsAsync();
        var unreadCount = (await _contactService.GetUnreadContactsAsync()).Count();
        
        ViewBag.UnreadCount = unreadCount;
        ViewBag.TotalCount = contacts.Count();
        
        return View(contacts);
    }
}

