using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    [Authorize]
    public async Task<IActionResult> Shorten([FromBody] string originalUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _userManager.FindByIdAsync(userId!);

        var shortCode = await _urlService.ShortenUrlAsync(originalUrl, user);
        
        return Ok(new { shortCode });
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetOriginal(string code)
    {
        var originalUrl = await _urlService.GetOriginalUrlAsync(code);
        if (originalUrl == null) return NotFound();
        return Ok(new { originalUrl });
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<IActionResult> GetHistory()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var records = await _urlService.GetUrlsByUserIdAsync(userId);
        return Ok(records);
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
}