using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces;

public interface IUrlService
{
    Task<string> ShortenUrlAsync(string originalUrl, User? user);

    Task<string> GetOriginalUrlAsync(string shortCode);
    Task<List<UrlRecord>> GetUrlsByUserIdAsync(string userId);
}