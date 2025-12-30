using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IImageCommandService
{
    Task<ImageResponse?> UploadImageAsync(ImageRegisterRequest request, UserCredential userCredential);
    Task<bool> DeleteImageAsync(ImageDeleteRequest request, UserCredential userCredential);
    Task<bool> SetMainImageAsync(ImageSetMainRequest request, UserCredential userCredential);
    Task<bool> UpdateImageOrderAsync(ImageUpdateOrderRequest request, UserCredential userCredential);
    Task<bool> UpdateImageAltAsync(ImageUpdateAltRequest request, UserCredential userCredential);
    Task<bool> DeleteEntityImagesAsync(EEntityType entityType, long entityId, UserCredential userCredential);
    Task<Stream?> DownloadImageAsync(long imageId);
}