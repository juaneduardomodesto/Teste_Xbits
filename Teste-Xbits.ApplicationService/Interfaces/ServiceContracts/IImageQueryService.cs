using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IImageQueryService
{
    Task<ImageResponse?> FindByIdAsync(long id);
    Task<IEnumerable<ImageResponse>> FindByEntityAsync(EEntityType entityType, long entityId);
    Task<ImageResponse?> GetMainImageAsync(EEntityType entityType, long entityId);
}