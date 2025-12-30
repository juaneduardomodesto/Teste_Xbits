using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public class ImageMapping : MappingBase , IEntityTypeConfiguration<ImageFiles>
{
    public void Configure(EntityTypeBuilder<ImageFiles> builder)
    {
        builder.ToTable(nameof(ImageFiles), Schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.StoragePath).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(x => x.SizeInBytes).IsRequired();
        builder.Property(x => x.ImageType).IsRequired();
        builder.Property(x => x.EntityType).IsRequired();
        builder.Property(x => x.EntityId).IsRequired();
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.Property(x => x.IsMain).HasDefaultValue(false);
        builder.Property(x => x.Alt).HasMaxLength(500);
        
        builder.Property(x => x.OriginalUrl).HasMaxLength(1000);
        builder.Property(x => x.ThumbnailUrl).HasMaxLength(1000);
        builder.Property(x => x.MediumUrl).HasMaxLength(1000);
        builder.Property(x => x.LargeUrl).HasMaxLength(1000);
        
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.HasIndex(x => new { x.EntityType, x.EntityId });
        builder.HasIndex(x => new { x.EntityType, x.EntityId, x.IsMain });
        builder.HasIndex(x => x.StoragePath);
    }
}