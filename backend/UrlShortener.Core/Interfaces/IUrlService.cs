using UrlShortener.Core.Entities;
using UrlShortener.Core.DTOs;

namespace UrlShortener.Core.Interfaces;

public interface IUrlService
{
    Task<UrlRecord> ShortenUrlAsync(string originalUrl, User? user);
    Task<string?> GetOriginalUrlAsync(string code);
    Task<List<UrlRecord>> GetUrlsByUserIdAsync(string userId); 
    Task<UrlRecord?> GetUrlByIdAsync(int id);
    Task<bool> DeleteUrlAsync(int id, string userId);
    Task<IEnumerable<UrlDto>> GetAllUrlsAsync();
}