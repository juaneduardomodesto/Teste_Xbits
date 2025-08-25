using Teste_Xbits.Domain.Entities.Base;

namespace Teste_Xbits.Domain.Entities;

public record Login: BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
}