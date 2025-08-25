namespace Teste_Xbits.Domain.Entities.Base;

public record BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}