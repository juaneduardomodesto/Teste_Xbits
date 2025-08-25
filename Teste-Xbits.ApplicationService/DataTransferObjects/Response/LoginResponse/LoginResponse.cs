namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;

public record LoginResponse
{
    public string? Username { get; set; }
    public string? Token { get; set; }
    public long? ExpireIn { get; set; }
}