using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;

namespace Teste_Xbits.Domain.EntitiesValidation;

public sealed class UserValidation : Validate<User>
{
    public UserValidation()
    {
        SetRules();
    }

    void SetRules()
    {
        
    }
}