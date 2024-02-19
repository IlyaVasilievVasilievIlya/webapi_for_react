using Cars.Api.Settings;
using LearnProject.Shared.Common;

namespace Cars.Api.Configuration
{
    public static class ConfigureExternalProviders
    {
        public static IServiceCollection ConfigureExternalProviers(this IServiceCollection services, IConfiguration config)
        {

            var settings = new ExternalProviders();
            var section = config.GetSection(nameof(ExternalProviders));
            section.Bind(settings);
            services.AddSingleton(settings);

            return services;
        }
    }
}
