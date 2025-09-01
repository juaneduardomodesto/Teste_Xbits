using Teste_Xbits.Domain.Entities.Base;

namespace Teste_Xbits.Domain.Entities;

public record ProductCategory : BaseEntity
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string ProductCategoryCode { get; init; }
    public virtual ICollection<Product> Products { get; init; } = new List<Product>();
}