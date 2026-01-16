using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;

public record LoginResponse
{
    public string? UserIdentifier { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    public string? Token { get; set; }
    public long? ExpireIn { get; set; }
}