namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;

public record ImageUploadResult(
    string StoragePath, 
    string PublicUrl, 
    long SizeInBytes);