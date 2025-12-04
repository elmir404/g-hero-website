using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;
using GHeroWebsite.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GHeroWebsite.Application.Services;

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;

    public ContactService(ApplicationDbContext context)
    {
        _context = context;
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
        return contact.Id;
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        return await _context.Contacts
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
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

