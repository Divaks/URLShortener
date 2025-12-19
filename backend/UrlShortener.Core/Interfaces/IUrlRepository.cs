using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces;

public interface IUrlRepository
{
    Task<bool> IsShortCodeTakenAsync(string shortCode);
    Task<UrlRecord?> GetByShortCodeAsync(string shortCode);
    Task AddAsync(UrlRecord record);
    Task SaveChangesAsync();
    Task<List<UrlRecord>> GetUrlsByUserIdAsync(string userId);
    Task<UrlRecord?> GetUrlByIdAsync(int id);
    Task DeleteAsync(UrlRecord record);
    Task<List<UrlRecord>> GetAllUrlsAsync();
    Task<UrlRecord?> GetByCodeAsync(string code);
    Task IncrementClickCountAsync(int id);
}