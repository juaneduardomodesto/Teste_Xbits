using Teste_Xbits.Domain.Entities.Base;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.Domain.Entities;

public record ImageFiles : BaseEntity
{
    public long Id { get; init; }
    
    public required string FileName { get; init; }
    
    public required string StoragePath { get; init; }
    
    public required string ContentType { get; init; }
    
    public long SizeInBytes { get; init; }
    
    public required EImageType ImageType { get; init; }
    
    public required EEntityType EntityType { get; init; }
    
    public required long EntityId { get; init; }
    
    public int DisplayOrder { get; init; } = 0;
    
    public bool IsMain { get; init; } = false;
    
    public string? Alt { get; init; }
    
    public string? Url { get; init; }
    
    public string? OriginalUrl { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? MediumUrl { get; init; }
    public string? LargeUrl { get; init; }
}