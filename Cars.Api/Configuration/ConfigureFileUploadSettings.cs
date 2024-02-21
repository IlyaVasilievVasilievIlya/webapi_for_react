using Cars.Api.Settings;
using LearnProject.Shared.Common;

namespace Cars.Api.Configuration
{
    public static class ConfigureFileUploadSettings
    {
        public static IServiceCollection AddFileUploadSettings(this IServiceCollection services, IConfiguration config)
        {
            var settings = new FileUploadSettings();
            var section = config.GetSection(nameof(FileUploadSettings));
            section.Bind(settings);
            services.AddSingleton(settings);

            return services;
        }
    }
}
