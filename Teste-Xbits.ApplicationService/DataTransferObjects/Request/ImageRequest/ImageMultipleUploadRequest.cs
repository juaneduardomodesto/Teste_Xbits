using Microsoft.AspNetCore.Http;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;

public record ImageMultipleUploadRequest
{
    public required List<IFormFile> Files { get; init; }
    public required EEntityType EntityType { get; init; }
    public required long EntityId { get; init; }
    public required EImageType ImageType { get; init; }
    public bool SetFirstAsMain { get; init; } = true;
    public List<string>? Alts { get; init; }
}