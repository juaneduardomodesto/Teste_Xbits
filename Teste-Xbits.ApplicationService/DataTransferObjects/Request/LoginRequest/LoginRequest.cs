namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;

public record LoginRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}