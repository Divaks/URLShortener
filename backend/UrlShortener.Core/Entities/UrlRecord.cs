namespace UrlShortener.Core.Entities;

public class UrlRecord
{
    public int Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortCode { get; set; }
    public DateTime DateCreated { get; set; }
    public required string UserId { get; set; }
    public required string CreatedBy { get; set; }
}