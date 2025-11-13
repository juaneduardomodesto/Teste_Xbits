using Microsoft.EntityFrameworkCore;
using Teste_Xbits.Infra.ORM.DataSeeds;

namespace Teste_Xbits.Infra.ORM.Context;

public sealed class DbInitializer
{
    private readonly ApplicationContext _applicationContext;

    public DbInitializer(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Seeding()
    {
        if (!await _applicationContext.Users.AnyAsync())
        {
            var userSeed = UserSeed.CreateSeed();
            await _applicationContext.Users.AddAsync(userSeed);
            await _applicationContext.SaveChangesAsync();
        }
    }
}