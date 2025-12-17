using System.Security.Cryptography;
using System.Text;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Application.Services;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _repository;

    public UrlService(IUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> ShortenUrlAsync(string originalUrl, User? user)
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

        // 👇 Додаємо і зберігаємо через репозиторій
        await _repository.AddAsync(record);
        await _repository.SaveChangesAsync();

        return code;
    }

    public async Task<string?> GetOriginalUrlAsync(string shortCode)
    {
        var record = await _repository.GetByShortCodeAsync(shortCode);
        return record?.OriginalUrl;
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
}