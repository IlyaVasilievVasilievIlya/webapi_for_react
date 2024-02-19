using Cars.Api.Settings;
using LearnProject.Shared.Common;
//using Minio;
using Minio.AspNetCore;

namespace Cars.Api.Configuration
{
    public static class ConfigureMinio
    {
        public static IServiceCollection AddAppMinio(this IServiceCollection services, IConfiguration config)
        {

            var settings = new MinioSettings();
            var section = config.GetSection(nameof(MinioSettings));
            section.Bind(settings);
            services.AddSingleton(settings);

            if (settings.SecretKey != null && settings.AccessKey != null)
            {
                services.AddMinio(new Uri($"http://{settings.AccessKey}:{settings.SecretKey}@{settings.Endpoint}"));
            }

            return services;
        }
    }
}
