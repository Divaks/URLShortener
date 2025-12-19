using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Web.Models;

namespace UrlShortener.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly UserManager<User> _userManager;

    public UrlController(IUrlService urlService, UserManager<User> userManager)
    {
        _urlService = urlService;
        _userManager = userManager;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> Shorten([FromBody] CreateUrlRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OriginalUrl))
        {
            return BadRequest("URL cannot be empty.");
        }

        var user = await _userManager.GetUserAsync(User);

        var record = await _urlService.ShortenUrlAsync(request.OriginalUrl, user);
        
        return Ok(new 
        { 
            id = record.Id,
            originalUrl = record.OriginalUrl,
            shortCode = record.ShortCode,
            dateCreated = record.DateCreated,
            createdBy = record.CreatedBy
        });
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetOriginal(string code)
    {
        var originalUrl = await _urlService.GetOriginalUrlAsync(code);
        if (originalUrl == null) return NotFound();
        return Ok(new { originalUrl });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            var allUrls = await _urlService.GetAllUrlsAsync();
            return Ok(allUrls);
        }
        else
        {
            var userUrls = await _urlService.GetUrlsByUserIdAsync(user.Id);
            return Ok(userUrls);
        }
    }
    
    [HttpGet("/go/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var originalUrl = await _urlService.GetOriginalUrlAsync(code);
        
        if (originalUrl == null)
        {
            return NotFound();
        }

        return Redirect(originalUrl);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var isDeleted = await _urlService.DeleteUrlAsync(id, userId);

        if (!isDeleted)
        {
            return BadRequest("Unable to delete URL (not found or access denied).");
        }

        return NoContent();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUrlDetails(int id)
    {
        var record = await _urlService.GetUrlByIdAsync(id);

        if (record == null) return NotFound();
        
        return Ok(new 
        {
            id = record.Id,
            originalUrl = record.OriginalUrl,
            shortCode = record.ShortCode,
            dateCreated = record.DateCreated,
            createdBy = record.CreatedBy
        });
    }
}