using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Infrastructure.Data;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly AppDbContext _context;
    
    public UrlRepository(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<bool> IsShortCodeTakenAsync(string shortCode)
    {
        return await _context.UrlRecords.AnyAsync(r => r.ShortCode == shortCode);
    }

    public async Task<UrlRecord?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.UrlRecords.FirstOrDefaultAsync(r => r.ShortCode == shortCode);
    }

    public async Task AddAsync(UrlRecord record)
    {
        await _context.UrlRecords.AddAsync(record);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<UrlRecord>> GetUrlsByUserIdAsync(string userId)
    {
        var records = await _context.UrlRecords.ToListAsync();
        return records;
    }
}