using Microsoft.AspNetCore.Http;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;

public record ImageRegisterRequest
{
    public required IFormFile File { get; init; }
    public required EEntityType EntityType { get; init; }
    public required long EntityId { get; init; }
    public required EImageType ImageType { get; init; }
    public bool SetAsMain { get; init; } = false;
    public string? Alt { get; init; }
}