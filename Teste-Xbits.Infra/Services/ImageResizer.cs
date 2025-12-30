using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;

namespace Teste_Xbits.Infra.Services;

public class ImageResizer : IImageResizerService
{
    public async Task<Stream> ResizeImageAsync(Stream imageStream, int maxWidth, int maxHeight)
    {
        using var image = await Image.LoadAsync(imageStream);
        
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(maxWidth, maxHeight),
            Mode = ResizeMode.Max
        }));

        var outputStream = new MemoryStream();
        await image.SaveAsJpegAsync(outputStream);
        outputStream.Position = 0;
        
        return outputStream;
    }

    public async Task<Stream> CreateThumbnailAsync(Stream imageStream, int size)
    {
        return await ResizeImageAsync(imageStream, size, size);
    }
}