using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHeroWebsite.WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/contact")]
[Authorize(Roles = "Admin")]
public class ContactMessagesController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactMessagesController> _logger;

    public ContactMessagesController(IContactService contactService, ILogger<ContactMessagesController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    /// <summary>
    /// Get all contact messages
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var contacts = await _contactService.GetAllContactsAsync();
        var total = contacts.Count();
        var pagedContacts = contacts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ContactResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Subject = c.Subject,
                Company = c.Company,
                Message = c.Message,
                CreatedAt = c.CreatedAt,
                IsRead = c.IsRead,
                ReadAt = c.ReadAt
            })
            .ToList();

        return Ok(new
        {
            data = pagedContacts,
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    /// <summary>
    /// Get unread contact messages
    /// </summary>
    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var contacts = await _contactService.GetUnreadContactsAsync();
        var result = contacts.Select(c => new ContactResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Subject = c.Subject,
            Company = c.Company,
            Message = c.Message,
            CreatedAt = c.CreatedAt,
            IsRead = c.IsRead,
            ReadAt = c.ReadAt
        }).ToList();

        return Ok(new { data = result, count = result.Count });
    }

    /// <summary>
    /// Get contact message by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound(new { message = "Contact message not found" });
        }

        var result = new ContactResponseDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Subject = contact.Subject,
            Company = contact.Company,
            Message = contact.Message,
            CreatedAt = contact.CreatedAt,
            IsRead = contact.IsRead,
            ReadAt = contact.ReadAt
        };

        return Ok(result);
    }

    /// <summary>
    /// Mark contact message as read
    /// </summary>
    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _contactService.MarkAsReadAsync(id);
        return Ok(new { message = "Message marked as read" });
    }

    /// <summary>
    /// Mark contact message as unread
    /// </summary>
    [HttpPatch("{id}/unread")]
    public async Task<IActionResult> MarkAsUnread(int id)
    {
        await _contactService.MarkAsUnreadAsync(id);
        return Ok(new { message = "Message marked as unread" });
    }

    /// <summary>
    /// Delete contact message
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _contactService.DeleteContactAsync(id);
            return Ok(new { message = "Message deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contact message {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the message" });
        }
    }
}

