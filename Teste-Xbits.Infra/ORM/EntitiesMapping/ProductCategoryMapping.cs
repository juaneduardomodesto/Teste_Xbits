using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public sealed class ProductCategoryMapping : MappingBase, IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable(nameof(ProductCategory), Schema);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(u => u.Id)
            .HasColumnType("bigint")
            .HasColumnName("product_category_id")
            .ValueGeneratedOnAdd();
        
        builder.Property(u => u.Name)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("name")
            .IsRequired(true);
        
        builder.Property(u => u.Description)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("description")
            .IsRequired(true);
        
        builder.Property(u => u.ProductCategoryCode)
            .HasColumnType("nvarchar(450)")
            .HasColumnName("product_category_code")
            .IsRequired(true);
        
        builder.HasIndex(u => u.ProductCategoryCode)
            .IsUnique()
            .HasDatabaseName("IX_ProductCategory_ProductCategoryCode_Unique");
        
        builder.HasMany(pc => pc.Products)
            .WithOne(p => p.ProductCategory)
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_Product_Name_Unique");
    }
}