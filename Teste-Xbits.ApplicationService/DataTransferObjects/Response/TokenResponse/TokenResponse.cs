namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;

public record TokenResponse
{
    public required string AccessToken { get; set; }
    public required string UserIdentifier { get; set; }
    public int ExpireIn { get; set; }
}