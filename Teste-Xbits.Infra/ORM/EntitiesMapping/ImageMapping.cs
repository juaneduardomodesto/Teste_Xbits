using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public class ImageMapping : MappingBase, IEntityTypeConfiguration<ImageFiles>
{
    public void Configure(EntityTypeBuilder<ImageFiles> builder)
    {
        builder.ToTable(nameof(ImageFiles), Schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnType("bigint")
            .HasColumnName("image_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FileName)
            .HasColumnType("nvarchar(255)")
            .HasColumnName("file_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.StoragePath)
            .HasColumnType("nvarchar(500)")
            .HasColumnName("storage_path")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .HasColumnType("nvarchar(100)")
            .HasColumnName("content_type")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.SizeInBytes)
            .HasColumnType("bigint")
            .HasColumnName("size_in_bytes")
            .IsRequired();

        builder.Property(x => x.ImageType)
            .HasColumnType("int")
            .HasColumnName("image_type")
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.EntityType)
            .HasColumnType("int")
            .HasColumnName("entity_type")
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.EntityId)
            .HasColumnType("bigint")
            .HasColumnName("entity_id")
            .IsRequired();

        builder.Property(x => x.DisplayOrder)
            .HasColumnType("int")
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(x => x.IsMain)
            .HasColumnType("bit")
            .HasColumnName("is_main")
            .HasDefaultValue(false);

        builder.Property(x => x.Alt)
            .HasColumnType("nvarchar(500)")
            .HasColumnName("alt_text")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.Url)
            .HasColumnType("nvarchar(1000)")
            .HasColumnName("url")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.OriginalUrl)
            .HasColumnType("nvarchar(1000)")
            .HasColumnName("original_url")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.ThumbnailUrl)
            .HasColumnType("nvarchar(1000)")
            .HasColumnName("thumbnail_url")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.MediumUrl)
            .HasColumnType("nvarchar(1000)")
            .HasColumnName("medium_url")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.LargeUrl)
            .HasColumnType("nvarchar(1000)")
            .HasColumnName("large_url")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(x => new { x.EntityType, x.EntityId })
            .HasDatabaseName("IX_ImageFiles_EntityType_EntityId");

        builder.HasIndex(x => new { x.EntityType, x.EntityId, x.IsMain })
            .HasDatabaseName("IX_ImageFiles_EntityType_EntityId_IsMain");

        builder.HasIndex(x => x.StoragePath)
            .HasDatabaseName("IX_ImageFiles_StoragePath");

        // Índice adicional para otimização
        builder.HasIndex(x => x.ImageType)
            .HasDatabaseName("IX_ImageFiles_ImageType");
    }
}