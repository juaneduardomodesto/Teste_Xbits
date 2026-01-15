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
        // Seed Users
        if (!await _applicationContext.Users.AnyAsync())
        {
            var userSeeds = UserSeed.CreateSeeds();
            await _applicationContext.Users.AddRangeAsync(userSeeds);
            await _applicationContext.SaveChangesAsync();
        }

        // Seed Product Categories
        if (!await _applicationContext.ProductCategories.AnyAsync())
        {
            var categoriesSeeds = ProductCategorySeed.CreateSeeds();
            await _applicationContext.ProductCategories.AddRangeAsync(categoriesSeeds);
            await _applicationContext.SaveChangesAsync();
        }

        // Seed Products
        if (!await _applicationContext.Products.AnyAsync())
        {
            var productsSeeds = ProductSeed.CreateSeeds();
            await _applicationContext.Products.AddRangeAsync(productsSeeds);
            await _applicationContext.SaveChangesAsync();
        }
    }
}