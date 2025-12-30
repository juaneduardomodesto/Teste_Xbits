using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IImageStorageService
{
    Task<ImageUploadResult> UploadAsync(
        Stream imageStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string storagePath, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string storagePath, CancellationToken cancellationToken = default);
    string GetPublicUrl(string storagePath);
}