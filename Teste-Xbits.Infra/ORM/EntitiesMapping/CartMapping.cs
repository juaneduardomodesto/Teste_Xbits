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
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CheckedOutAt);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        // Ignorar a propriedade de navegação User (não é necessária)
        builder.Ignore(x => x.User);
        // Ignorar propriedades calculadas
        builder.Ignore(x => x.Subtotal);
        builder.Ignore(x => x.TotalItems);

        builder.HasIndex(x => new { x.UserId, x.Status });
    }
}