using System.Data.Entity;
using Teste_Xbits.Infra.ORM.DataSeeds;

namespace Teste_Xbits.Infra.ORM.Context;

public sealed class DbInitializer(
    ApplicationContext applicationContext)
{
    public async Task Seeding()
    {
        if (!applicationContext.Users.Any())
        {
            var userSeed = UserSeed.CreateSeed();
            applicationContext.Users.Add(userSeed);
            applicationContext.SaveChanges();
        }
    }
}