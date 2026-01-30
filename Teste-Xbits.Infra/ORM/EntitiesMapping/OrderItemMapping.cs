using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public class OrderItemMapping : MappingBase, IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem), Schema);

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnType("bigint")
            .HasColumnName("order_item_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.OrderId)
            .HasColumnType("bigint")
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.ProductId)
            .HasColumnType("bigint")
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.ProductName)
            .HasColumnType("nvarchar(255)")
            .HasColumnName("product_name")
            .HasMaxLength(255)
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

        builder.HasOne(x => x.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("IX_OrderItem_OrderId");

        // Índice composto para consultas frequentes
        builder.HasIndex(x => new { x.OrderId, x.ProductId })
            .HasDatabaseName("IX_OrderItem_OrderId_ProductId");
    }
}