using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data.DAL.Configuration
{
}    /// <summary>
     /// Ограничения таблицы refresh токенов
     /// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(entity => entity.Token);

        builder.Property(e => e.Token)
            .HasColumnName("token")
            .HasComment("token");

        builder.Property(e => e.CreationDate)
            .HasColumnName("creation_date")
            .HasComment("token creation date");

        builder.Property(e => e.ExpiryDate)
            .HasColumnName("expiry_date")
            .HasComment("token expiry date");

        builder.Property(entity => entity.UserId)
            .HasColumnName("user_id")
            .HasComment("user id");

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId);
    }
}
