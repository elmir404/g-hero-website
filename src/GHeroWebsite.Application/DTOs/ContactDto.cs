using System.ComponentModel.DataAnnotations;

namespace GHeroWebsite.Application.DTOs;

public class ContactDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    public string Email { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Company cannot exceed 200 characters")]
    public string? Company { get; set; }

    [Required(ErrorMessage = "Message is required")]
    [StringLength(5000, ErrorMessage = "Message cannot exceed 5000 characters")]
    public string Message { get; set; } = string.Empty;
}

