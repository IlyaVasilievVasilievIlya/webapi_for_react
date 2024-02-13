using LearnProject.BLL.Contracts;
using LearnProject.BLL.Services;
using LearnProject.BLL.Services.Services;
using LearnProject.Data.DAL.Repositories;
using LearnProject.Data.MigrationService;
using LearnProject.Domain.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Cars.Api.Configuration
{
    internal static class Bootstrapper
    {
        /// <summary>
        /// метод расширения для подключения сервисов из бизнес-слоя
        /// </summary>
        /// <param name="services">коллекция DI</param>
        /// <returns>коллекция DI</returns>
        internal static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarModelRepository, CarModelRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMigrationService, MigrationService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
