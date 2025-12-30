using System.ComponentModel;

namespace Teste_Xbits.Domain.Enums;

public enum EImageContentType : uint
{
    [Description("image/jpeg")]
    Jpeg = 1,
    [Description("image/jpg")]
    Jpg = 2,
    [Description("image/png")]
    Png = 3,
    [Description("image/webp")]
    Webp = 4
}

public static class EImageContentTypeExtensions
{
    private static readonly Dictionary<EImageContentType, string> ContentTypeMap = new()
    {
        { EImageContentType.Jpeg, "image/jpeg" },
        { EImageContentType.Jpg, "image/jpg" },
        { EImageContentType.Png, "image/png" },
        { EImageContentType.Webp, "image/webp" }
    };

    public static string GetContentType(this EImageContentType contentType)
    {
        return ContentTypeMap.GetValueOrDefault(contentType, "image/jpeg");
    }

    public static string[] GetAllowedContentTypes()
    {
        return ContentTypeMap.Values.ToArray();
    }

    public static bool IsValidContentType(string contentType)
    {
        return ContentTypeMap.Values.Contains(contentType.ToLower());
    }

    public static EImageContentType? FromContentType(string contentType)
    {
        return ContentTypeMap.FirstOrDefault(x => x.Value.Equals(contentType, StringComparison.OrdinalIgnoreCase)).Key;
    }
}