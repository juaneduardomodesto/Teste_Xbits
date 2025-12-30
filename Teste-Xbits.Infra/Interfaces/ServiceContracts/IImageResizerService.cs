namespace Teste_Xbits.Infra.Interfaces.ServiceContracts;

public interface IImageResizerService
{
    Task<Stream> ResizeImageAsync(Stream imageStream, int maxWidth, int maxHeight);
    Task<Stream> CreateThumbnailAsync(Stream imageStream, int size);
}