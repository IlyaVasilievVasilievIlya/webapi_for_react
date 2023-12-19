using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnProject.Data.DAL.Configuration
{
    /// <summary>
    /// Ограничения таблицы пользователей
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", t => t.HasComment("users table"));

            builder.Property(entity => entity.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("user name");

            builder.Property(entity => entity.Surname)
                .IsRequired()
                .HasColumnName("surname")
                .HasMaxLength(100)
                .HasComment("user surname");

            builder.Property(entity => entity.Patronymic)
                .HasMaxLength(100)
                .HasColumnName("patronymic")
                .HasComment("user patronymic");

            builder.Property(entity => entity.BirthDate)
                .HasColumnName("birth_date")
                .HasComment("user birth date");
        }
    }
}
