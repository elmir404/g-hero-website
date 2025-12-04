using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.Web.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class ContactMessagesController : Controller
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactMessagesController> _logger;

    public ContactMessagesController(IContactService contactService, ILogger<ContactMessagesController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var contacts = await _contactService.GetAllContactsAsync();
        return View(contacts);
    }

    public async Task<IActionResult> Details(int id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        // Mark as read when viewing
        if (!contact.IsRead)
        {
            await _contactService.MarkAsReadAsync(id);
        }

        return View(contact);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _contactService.MarkAsReadAsync(id);
        TempData["Success"] = "Message marked as read.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsUnread(int id)
    {
        await _contactService.MarkAsUnreadAsync(id);
        TempData["Success"] = "Message marked as unread.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _contactService.DeleteContactAsync(id);
            TempData["Success"] = "Message deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contact message {Id}", id);
            TempData["Error"] = "An error occurred while deleting the message.";
        }

        return RedirectToAction(nameof(Index));
    }
}

