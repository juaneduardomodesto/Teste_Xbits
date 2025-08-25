using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class TokenValidation : Validate<Token>
{
    public TokenValidation()
    {
        SetRules();
    }

    void SetRules()
    {
        
    }
}