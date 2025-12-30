using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;

public record ImageResponse
{
    public long Id { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeInBytes { get; init; }
    public EImageType ImageType { get; init; }
    public bool IsMain { get; init; }
    public string? Alt { get; init; }
    public int DisplayOrder { get; init; }
    
    public string? OriginalUrl { get; init; }
    public string? ThumbnailUrl { get; init; }  // 150x150
    public string? MediumUrl { get; init; }     // 800x800
    public string? LargeUrl { get; init; }      // 1920x1920
    
    public DateTime CreatedAt { get; init; }
    
    public string GetUrlForSize(string size)
    {
        return size.ToLower() switch
        {
            "thumbnail" or "thumb" => ThumbnailUrl ?? MediumUrl ?? LargeUrl ?? OriginalUrl ?? string.Empty,
            "medium" => MediumUrl ?? LargeUrl ?? OriginalUrl ?? string.Empty,
            "large" => LargeUrl ?? OriginalUrl ?? string.Empty,
            "original" => OriginalUrl ?? string.Empty,
            _ => OriginalUrl ?? string.Empty
        };
    }
}