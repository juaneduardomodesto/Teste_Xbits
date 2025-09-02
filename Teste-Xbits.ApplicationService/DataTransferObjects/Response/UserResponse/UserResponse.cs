using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;

public class UserResponse
{
    public long? Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
    
    public required string Cpf { get; init; }
    
    public required bool AcceptPrivacyPolicy { get; init; }

    public required bool AcceptTermsOfUse { get; init; }
    
    public bool IsActive { get; set; } = true;
    
    public ERoles Role { get; init; }
}