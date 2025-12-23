using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public class ProductMapping : MappingBase, IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product), Schema);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnType("bigint")
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired(true);
        
        builder.Property(x => x.Name)
            .HasColumnType("nvarchar(200)")
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired(true);
        
        builder.Property(x => x.Description)
            .HasColumnType("nvarchar(1000)")
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired(false);
        
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("price")
            .IsRequired(true);
        
        builder.Property(x => x.Code)
            .HasColumnType("nvarchar(50)")
            .HasColumnName("product_code")
            .HasMaxLength(50)
            .IsRequired(true);
        
        builder.Property(x => x.HasExpirationDate)
            .HasColumnType("bit")
            .HasColumnName("has_expiration_date")
            .IsRequired(true);
        
        builder.Property(x => x.ExpirationDate)
            .HasColumnType("datetime")
            .HasColumnName("expiration_date")
            .IsRequired(false);
        
        builder.Property(x => x.ProductCategoryId)
            .HasColumnType("bigint")
            .HasColumnName("product_category_id")
            .IsRequired(true);
        
        builder.HasOne(p => p.ProductCategory)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.ProductCategoryId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_Product_Name_Unique");
    }
}