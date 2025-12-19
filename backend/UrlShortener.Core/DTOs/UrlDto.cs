namespace UrlShortener.Core.DTOs;

public class UrlDto
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int ClickCount { get; set; }
}