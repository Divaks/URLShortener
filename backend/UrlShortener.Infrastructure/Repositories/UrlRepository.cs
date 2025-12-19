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
        return await _context.UrlRecords
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.DateCreated)
            .ToListAsync();
    }

    public async Task<UrlRecord?> GetUrlByIdAsync(int id)
    {
        return await _context.UrlRecords.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task DeleteAsync(UrlRecord record)
    {
        _context.UrlRecords.Remove(record);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<UrlRecord>> GetAllUrlsAsync()
    {
        return await _context.UrlRecords
            .AsNoTracking()
            .OrderByDescending(r => r.DateCreated)
            .ToListAsync();
    }
    
    public async Task<UrlRecord?> GetByCodeAsync(string code)
    {
        return await _context.UrlRecords.FirstOrDefaultAsync(u => u.ShortCode == code);
    }

    public async Task IncrementClickCountAsync(int id)
    {
        var record = await _context.UrlRecords.FindAsync(id);
        if (record != null)
        {
            record.ClickCount++;
            await _context.SaveChangesAsync();
        }
    }
}