namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;

public record TokenResponse
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public string Username { get; set; }
    public int ExpireIn { get; set; }
}