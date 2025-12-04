using GHeroWebsite.Application.DTOs;
using GHeroWebsite.Domain.Entities;

namespace GHeroWebsite.Application.Services;

public interface IContactService
{
    Task<int> CreateContactAsync(ContactRequestDto contactDto);
    Task<Contact?> GetContactByIdAsync(int id);
    Task<IEnumerable<Contact>> GetAllContactsAsync();
    Task<IEnumerable<Contact>> GetUnreadContactsAsync();
    Task MarkAsReadAsync(int id);
    Task MarkAsUnreadAsync(int id);
    Task DeleteContactAsync(int id);
}

