using Microsoft.Extensions.Configuration;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

namespace Teste_Xbits.ApplicationService.Services.Storage;

public class LocalImageStorageService : IImageStorageService
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    public LocalImageStorageService(IConfiguration configuration)
    {
        _basePath = configuration["Storage:BasePath"] ?? "wwwroot/uploads";
        _baseUrl = configuration["Storage:BaseUrl"] ?? "/uploads";
    }

    public async Task<ImageUploadResult> UploadAsync(
        Stream imageStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var relativePath = Path.Combine(
            DateTime.UtcNow.Year.ToString(),
            DateTime.UtcNow.Month.ToString("D2"),
            uniqueFileName);

        var fullPath = Path.Combine(_basePath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var fileStream = new FileStream(fullPath, FileMode.Create);
        await imageStream.CopyToAsync(fileStream, cancellationToken);

        var sizeInBytes = fileStream.Length;
        var publicUrl = GetPublicUrl(relativePath);

        return new ImageUploadResult(relativePath, publicUrl, sizeInBytes);
    }

    public Task<bool> DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, storagePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<Stream?> DownloadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, storagePath);

        if (!File.Exists(fullPath))
        {
            return Task.FromResult<Stream?>(null);
        }

        return Task.FromResult<Stream?>(
            new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }

    public string GetPublicUrl(string storagePath)
    {
        return $"{_baseUrl}/{storagePath.Replace("\\", "/")}";
    }
}