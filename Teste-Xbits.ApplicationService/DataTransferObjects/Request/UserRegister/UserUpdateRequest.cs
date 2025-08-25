namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;

public record UserUpdateRequest : UserRegisterRequest
{
    public required long Id { get; init; }
}