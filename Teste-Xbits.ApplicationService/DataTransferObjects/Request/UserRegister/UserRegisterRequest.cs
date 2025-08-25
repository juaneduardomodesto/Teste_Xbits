using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;

public record UserRegisterRequest
{
    public required string Name { get; init; }
    
    public required string Email { get; init; }
    
    public required string Cpf { get; init; }
    
    public required string Password { get; init; }
    
    public required string ConfirmPassword { get; init; }
    
    public required bool AcceptPrivacyPolicy { get; set; }

    public required bool AcceptTermsOfUse { get; set; }
    
    public required ERoles Roles { get; init; }
}