using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public sealed class DomainLoggerMapping : MappingBase, IEntityTypeConfiguration<DomainLogger>
{
    public void Configure(EntityTypeBuilder<DomainLogger> builder)
    {
        builder.ToTable(nameof(DomainLogger), Schema);
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnType("bigint")
            .HasColumnName("id")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(l => l.Description)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("description")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(l => l.ActionDate)
            .HasColumnType("datetime2")
            .HasColumnName("date")
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(l => l.UserId)
            .HasColumnType("bigint")
            .HasColumnName("user_id")
            .HasColumnOrder(4)
            .IsRequired();

        builder.Property(l => l.EntityId)
            .HasColumnType("nvarchar(255)")
            .HasColumnName("entity_id")
            .HasColumnOrder(5)
            .IsRequired(false);
        
        builder.HasOne<Domain.Entities.User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}