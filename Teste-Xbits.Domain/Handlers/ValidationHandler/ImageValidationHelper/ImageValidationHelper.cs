using Microsoft.AspNetCore.Http;

namespace Teste_Xbits.Domain.Handlers.ValidationHandler.ImageValidationHelper;

public class ImageValidationHelper
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private static readonly string[] AllowedContentTypes = 
    { 
        "image/jpeg", 
        "image/jpg", 
        "image/png", 
        "image/webp" 
    };
    
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB
    private const int MaxFileNameLength = 255;

    public static (bool IsValid, string? ErrorMessage) ValidateImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return (false, "Arquivo de imagem é obrigatório");
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return (false, $"Extensão de arquivo não permitida. Use: {string.Join(", ", AllowedExtensions)}");
        
        if (!AllowedContentTypes.Contains(file.ContentType.ToLower()))
            return (false, $"Tipo de arquivo não permitido. Use: {string.Join(", ", AllowedContentTypes)}");
        
        if (file.Length > MaxFileSizeInBytes)
            return (false, $"Arquivo muito grande. Tamanho máximo: {MaxFileSizeInBytes / 1024 / 1024}MB");
        
        if (file.FileName.Length > MaxFileNameLength)
            return (false, $"Nome do arquivo muito longo. Máximo: {MaxFileNameLength} caracteres");
        
        var invalidChars = Path.GetInvalidFileNameChars();
        if (file.FileName.Any(c => invalidChars.Contains(c)))
            return (false, "Nome do arquivo contém caracteres inválidos");

        return (true, null);
    }

    public static bool IsImageFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }

    public static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars));
        
        if (sanitized.Length > MaxFileNameLength)
        {
            var extension = Path.GetExtension(sanitized);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(sanitized);
            sanitized = nameWithoutExtension.Substring(0, MaxFileNameLength - extension.Length) + extension;
        }

        return sanitized;
    }
}