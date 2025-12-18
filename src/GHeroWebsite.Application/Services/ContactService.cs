using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GHeroWebsite.Application.Services;

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactService> _logger;

    public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CreateContactAsync(ContactRequestDto contactDto)
    {
        var contact = new Contact
        {
            Name = contactDto.Name,
            Email = contactDto.Email,
            Subject = contactDto.Subject,
            Company = contactDto.Company,
            Message = contactDto.Message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        
        // Log successful creation (ID only for security)
        _logger.LogInformation("Contact message created successfully. ID: {ContactId}", contact.Id);
        
        return contact.Id;
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        var contacts = await _context.Contacts
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        
        _logger.LogInformation("Retrieved {Count} contact messages from database", contacts.Count());
        
        return contacts;
    }

    public async Task<IEnumerable<Contact>> GetUnreadContactsAsync()
    {
        return await _context.Contacts
            .Where(c => !c.IsRead)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            contact.IsRead = true;
            contact.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAsUnreadAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            contact.IsRead = false;
            contact.ReadAt = null;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}

