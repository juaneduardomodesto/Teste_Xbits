using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.Infra.ORM.DataSeeds;

public static class ProductSeed
{
    public static List<Product> CreateSeeds()
    {
        return new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Notebook Dell Inspiron",
                Description = "Notebook com processador Intel Core i5, 8GB RAM, 256GB SSD",
                Price = 3500.00m,
                Code = "DELL-NB-001",
                HasExpirationDate = false,
                ExpirationDate = null,
                ProductCategoryId = 1,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            },
            new Product
            {
                Id = 2,
                Name = "Mouse Logitech MX Master",
                Description = "Mouse sem fio ergonômico com sensor de alta precisão",
                Price = 450.00m,
                Code = "LOGI-MS-001",
                HasExpirationDate = false,
                ExpirationDate = null,
                ProductCategoryId = 1,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            },
            new Product
            {
                Id = 3,
                Name = "Leite Integral 1L",
                Description = "Leite integral pasteurizado",
                Price = 5.50m,
                Code = "LEITE-001",
                HasExpirationDate = true,
                ExpirationDate = DateTime.Today.AddDays(7),
                ProductCategoryId = 2,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            },
            new Product
            {
                Id = 4,
                Name = "Pão de Forma",
                Description = "Pão de forma integral com 12 fatias",
                Price = 8.90m,
                Code = "PAO-001",
                HasExpirationDate = true,
                ExpirationDate = DateTime.Today.AddDays(5),
                ProductCategoryId = 2,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            },
            new Product
            {
                Id = 5,
                Name = "Camiseta Básica",
                Description = "Camiseta 100% algodão, tamanho M",
                Price = 49.90m,
                Code = "CAM-001",
                HasExpirationDate = false,
                ExpirationDate = null,
                ProductCategoryId = 3,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            }
        };
    }
}