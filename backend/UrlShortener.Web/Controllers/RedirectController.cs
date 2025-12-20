using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Web.Controllers;

[ApiController]
[Route("s")]
public class RedirectController : ControllerBase
{
    private readonly IUrlService _urlService;

    public RedirectController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> Go(string code)
    {
        var originalUrl = await _urlService.GetOriginalUrlAsync(code);

        if (string.IsNullOrEmpty(originalUrl))
        {
            return NotFound("Посилання не знайдено 🕵️‍♂️");
        }

        return Redirect(originalUrl);
    }
}
