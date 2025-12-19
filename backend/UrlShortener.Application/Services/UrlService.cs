using System.Security.Cryptography;
using System.Text;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.DTOs;

namespace UrlShortener.Application.Services;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _repository;

    public UrlService(IUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<UrlRecord> ShortenUrlAsync(string originalUrl, User? user)
    {
        var code = GenerateHashCode(originalUrl);

        while (await _repository.IsShortCodeTakenAsync(code))
        {
            code = GenerateHashCode(originalUrl + Guid.NewGuid().ToString());
        }

        var record = new UrlRecord
        {
            OriginalUrl = originalUrl,
            ShortCode = code,
            UserId = user?.Id ?? "anonymous",
            CreatedBy = user?.UserName ?? "Anonymous",
            DateCreated = DateTime.UtcNow
        };

        await _repository.AddAsync(record);
        await _repository.SaveChangesAsync();

        return record;
    }

    public async Task<string?> GetOriginalUrlAsync(string code)
    {
        var record = await _repository.GetByCodeAsync(code);
    
        if (record == null)
        {
            return null;
        }

        await _repository.IncrementClickCountAsync(record.Id);

        return record.OriginalUrl;
    }

    private string GenerateHashCode(string input)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().Substring(0, 8);
        }
    }

    public async Task<List<UrlRecord>> GetUrlsByUserIdAsync(string userId)
    {
        var records = await _repository.GetUrlsByUserIdAsync(userId);
        return records;
    }
    
    public async Task<bool> DeleteUrlAsync(int id, string userId)
    {
        var record = await _repository.GetUrlByIdAsync(id);

        if (record == null)
        {
            return false;
        }

        if (record.UserId != userId)
        {
            return false;
        }

        await _repository.DeleteAsync(record);
        return true;
    }
    
    public async Task<UrlRecord?> GetUrlByIdAsync(int id)
    {
        return await _repository.GetUrlByIdAsync(id);
    }
    
    public async Task<IEnumerable<UrlDto>> GetAllUrlsAsync()
    {
        var records = await _repository.GetAllUrlsAsync();
    
        // 2. Перетворюємо їх у DTO
        return records.Select(r => new UrlDto
        {
            Id = r.Id,
            OriginalUrl = r.OriginalUrl,
            ShortCode = r.ShortCode,
            DateCreated = r.DateCreated,
            CreatedBy = r.CreatedBy,
            ClickCount = r.ClickCount,
        });
    }
}