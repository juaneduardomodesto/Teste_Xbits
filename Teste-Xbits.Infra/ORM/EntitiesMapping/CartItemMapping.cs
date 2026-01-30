using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public class CartItemMapping : MappingBase, IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable(nameof(CartItem), Schema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType("bigint")
            .HasColumnName("cart_item_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CartId)
            .HasColumnType("bigint")
            .HasColumnName("cart_id")
            .IsRequired();

        builder.Property(x => x.ProductId)
            .HasColumnType("bigint")
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnType("int")
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("unit_price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("updated_at")
            .IsRequired();

        // Ignorar propriedade calculada
        builder.Ignore(x => x.TotalPrice);

        builder.HasOne(x => x.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CartId, x.ProductId })
            .HasDatabaseName("IX_CartItem_CartId_ProductId");
    }
}