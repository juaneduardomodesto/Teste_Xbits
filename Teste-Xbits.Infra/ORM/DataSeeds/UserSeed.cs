using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Extensions;

namespace Teste_Xbits.Infra.ORM.DataSeeds;

public static class UserSeed
{
    private const string Salt = "i0aYsb+wrgu=pOi5-Qp3MPTm!Kt3nPL3*7dG!jaLb9-mP=pTNmhp-:7n_O_R9)Y0Efma0#Qg?q:Y4zL9#P!4rsZWyDGFqq(IICSc@09oDx3-Y7bWEZZ6RuBL";
    
    public static User CreateSeed()
    {
        return new User()
        {
            Name = "Joao",
            Cpf = "96582199005",
            Email = "Joao@gmail.com",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            PasswordHash = "12345678910abc".ConvertMd5(Salt),
            IsActive = true,
            Role = ERoles.Administrator,
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today,
        };
    }
}