using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.ORM.EntitiesMapping.Base;

namespace Teste_Xbits.Infra.ORM.EntitiesMapping;

public sealed class UserMapping : MappingBase, IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), Schema);

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasColumnType("bigint")
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Name)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("name")
            .IsRequired(true);
        
        builder.Property(u => u.Email)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("email")
            .IsRequired(true);

        builder.Property(u => u.Cpf)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("cpf")
            .IsRequired(true);
        
        builder.Property(u=> u.PasswordHash)
            .HasColumnType("nvarchar(500)")
            .HasColumnName("password_hash")
            .IsRequired(true);

        builder.Property(u => u.AcceptPrivacyPolicy)
            .HasColumnType("bit")
            .HasColumnName("accept_privacy_policy")
            .IsRequired(true);
        
        builder.Property(u => u.AcceptTermsOfUse)
            .HasColumnType("bit")
            .HasColumnName("accept_terms_of_use")
            .IsRequired(true);
    }
}