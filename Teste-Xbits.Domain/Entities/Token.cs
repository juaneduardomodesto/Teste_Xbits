using Teste_Xbits.Domain.Entities.Base;

namespace Teste_Xbits.Domain.Entities;

public record Token : BaseEntity
{
    public string AccessToken { get; set; }
    
    public string TokenType { get; set; } = "Bearer";
    
    public string Username { get; set; }
    
    public int ExpireIn { get; set; }  
    
    public long UserId { get; set; }
}