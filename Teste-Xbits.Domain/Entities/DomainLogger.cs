namespace Teste_Xbits.Domain.Entities;

public sealed class DomainLogger
{
    public long Id { get; init; }
    public DateTime ActionDate { get; init; }
    public required string Description { get; init; }
    public long UserId { get; init; }
    public string? EntityId { get; init; }
}