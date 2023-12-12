using LearnProject.Data.DAL.Configuration;
using LearnProject.Domain.Entities;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LearnProject.Data.DAL
{
    public class RepositoryAppDbContext : IdentityDbContext<User>
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarBrandModels { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public RepositoryAppDbContext(DbContextOptions<RepositoryAppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(RepositoryAppDbContext).Assembly);

            ToSnakeCase(builder);

            SeedData(builder);
        }

        /// <summary>
        /// перевод имен элементов схемы БД в snake_case
        /// </summary>
        /// <param name="builder"></param>
        static void ToSnakeCase(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

                foreach (var property in entity.GetProperties())
                {
                    var columnName = property.GetColumnName(StoreObjectIdentifier
                        .Table(property.DeclaringEntityType.GetTableName(), null));
                    property.SetColumnName(columnName?.ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()?.ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName()?.ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
                }
            }
        }

        /// <summary>
        /// заполнить начальными данными
        /// </summary>
        /// <param name="builder">класс настройки</param>
        static void SeedData(ModelBuilder builder)
        {
            builder.Entity<CarModel>().HasData(new CarModel { CarModelId = 1, Brand = "Toyota", Name = "Mark 1" });
            builder.Entity<CarModel>().HasData(new CarModel { CarModelId = 2, Brand = "Mercedes", Name = "Benz" });
            builder.Entity<CarModel>().HasData(new CarModel { CarModelId = 3, Brand = "Toyota", Name = "Mark 2" });
            builder.Entity<CarModel>().HasData(new CarModel { CarModelId = 4, Brand = "Renault", Name = "Logan" });

            CreateRolesAndSuperUser(builder);

            builder.Entity<Car>().HasData(new Car { CarId = 1, CarModelId = 1, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 2, CarModelId = 2, Color = "Black" });
            builder.Entity<Car>().HasData(new Car { CarId = 3, CarModelId = 2, Color = "White" });
            builder.Entity<Car>().HasData(new Car { CarId = 4, CarModelId = 3, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 5, CarModelId = 3, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 6, CarModelId = 1, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 7, CarModelId = 1, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 8, CarModelId = 4, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 9, CarModelId = 4, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 10, CarModelId = 4, Color = "Yellow" });
            builder.Entity<Car>().HasData(new Car { CarId = 11, CarModelId = 2, Color = "White" });
        }

        /// <summary>
        /// создание ролей и супепользователя
        /// </summary>
        /// <param name="builder">класс настройки</param>
        static void CreateRolesAndSuperUser(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = AppRoles.SuperUser, NormalizedName = AppRoles.SuperUser.ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7211", Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper()});
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = AppRoles.Manager, NormalizedName = AppRoles.Manager.ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7213", Name = AppRoles.User, NormalizedName = AppRoles.User.ToUpper() });
           
            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(
                new User
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    Name = "Ivan",
                    Surname = "Ivanov",
                    UserName = "ilyavasilev56@gmail.com",
                    BirthDate = new DateOnly(1999, 2, 2),
                    NormalizedUserName = "ilyavasilev56@gmail.com".ToUpper(),
                    Email = "ilyavasilev56@gmail.com",
                    NormalizedEmail = "ilyavasilev56@gmail.com".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "qwerty")
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
        }
    }
}
