using LearnProject.Data.DAL;
using LearnProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cars.Api.Configuration
{
    public static class ConfigureIdentity
    {
        public static IServiceCollection AddAppIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<RepositoryAppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
