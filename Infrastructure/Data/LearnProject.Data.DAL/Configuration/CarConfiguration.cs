using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnProject.Data.DAL.Configuration
{
    /// <summary>
    /// Ограничения таблицы авто
    /// </summary>
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(entity => entity.CarId);
            builder.Property(entity => entity.CarId)
                .HasColumnName("car_id")
                .HasComment("car id");

            builder.Property(entity => entity.CarModelId)
                .HasColumnName("car_model_id")
                .IsRequired()
                .HasComment("car model id");

            builder.Property(entity => entity.Color)
                .HasColumnName("color")
                .HasMaxLength(100)
                .HasComment("Car's color");

            builder.HasOne(car => car.CarModel)
                .WithMany()
                .HasForeignKey(car => car.CarModelId);
        }

    }
}
