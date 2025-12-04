using GHeroWebsite.Application.DTOs;

namespace GHeroWebsite.Application.Services;

public interface IHomePageService
{
    Task<HomePageDto> GetHomePageAsync(string languageCode = "en");
    Task<HomeHeroSectionDto?> GetHeroSectionAsync(string languageCode = "en");
    Task<List<HomeServiceDto>> GetServicesAsync(string languageCode = "en");
    Task<List<HomeTestimonialDto>> GetTestimonialsAsync(string languageCode = "en");
    Task<List<HomeFaqDto>> GetFaqsAsync(string languageCode = "en");
    Task<HomeContactInfoDto?> GetContactInfoAsync(string languageCode = "en");
}

