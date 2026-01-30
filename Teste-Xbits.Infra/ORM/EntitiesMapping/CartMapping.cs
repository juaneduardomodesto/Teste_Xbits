using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public class CartMapping : MappingBase, IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable(nameof(Cart), Schema);
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType("bigint")
            .HasColumnName("cart_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .HasColumnType("bigint")
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasColumnType("int")
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(x => x.CheckedOutAt)
            .HasColumnType("datetime2")
            .HasColumnName("checked_out_at")
            .IsRequired(false);
            
        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("created_at")
            .IsRequired();
            
        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("updated_at")
            .IsRequired();
        
        // Ignorar propriedades calculadas
        builder.Ignore(x => x.Subtotal);
        builder.Ignore(x => x.TotalItems);

        builder.HasIndex(x => new { x.UserId, x.Status })
            .HasDatabaseName("IX_Cart_UserId_Status");
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}