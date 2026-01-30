namespace Teste_Xbits.Domain.Entities;

public record UserCredential
{
    public long Id { get; init; }
    public required List<string> Roles { get; set; }
}