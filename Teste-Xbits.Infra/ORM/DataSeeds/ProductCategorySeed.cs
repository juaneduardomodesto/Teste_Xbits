using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.Infra.ORM.DataSeeds;

public static class ProductCategorySeed
{
    public static List<ProductCategory> CreateSeeds()
    {
        return new List<ProductCategory>
        {
            new ProductCategory
            {
                Name = "Eletrônicos",
                Description = "Produtos eletrônicos e tecnologia",
                ProductCategoryCode = "ELET001",
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            },
            new ProductCategory
            {
                Name = "Alimentos",
                Description = "Produtos alimentícios e bebidas",
                ProductCategoryCode = "ALIM001",
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            },
            new ProductCategory
            {
                Name = "Vestuário",
                Description = "Roupas e acessórios",
                ProductCategoryCode = "VEST001",
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            }
        };
    }
}