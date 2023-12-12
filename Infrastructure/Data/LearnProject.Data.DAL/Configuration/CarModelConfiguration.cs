using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnProject.Data.DAL.Configuration
{
    /// <summary>
    /// Ограничения таблицы модели модели авто
    /// </summary>
    public class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
    {
        public void Configure(EntityTypeBuilder<CarModel> builder)
        {
            builder.HasKey(entity => entity.CarModelId);
            builder.Property(e => e.CarModelId)
                .HasColumnName("car_id")
                .HasComment("car id");

            builder.Property(entity => entity.Brand)
                .HasColumnName("brand")
                .HasMaxLength(100)
                .HasComment("car brand");

            builder.Property(entity => entity.Name)
                .HasColumnName("model")
                .HasMaxLength(100)
                .HasComment("car model");
        }
    }
}
