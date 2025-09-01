using Teste_Xbits.Domain.Entities.Base;

namespace Teste_Xbits.Domain.Entities;

public record Product : BaseEntity
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public required string Code { get; init; }
    public bool HasExpirationDate { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public long? ProductCategoryId { get; init; }
    public virtual ProductCategory? ProductCategory { get; init; }
}