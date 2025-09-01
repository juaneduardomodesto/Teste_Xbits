namespace Teste_Xbits.Domain.Entities;

public record UserCredential
{
    public Guid Id { get; init; }
    public Guid? CompanyId { get; init; }
    public required List<string> Roles { get; set; }
}