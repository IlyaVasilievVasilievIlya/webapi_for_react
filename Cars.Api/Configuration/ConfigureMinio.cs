using LearnProject.Data.DAL;
using LearnProject.Data.MigrationService;
using LearnProject.Shared.Common;
using LearnProject.Shared.Common.Settings;
using Minio;
using Minio.AspNetCore;
using Minio.DataModel.Args;

namespace Cars.Api.Configuration
{
    /// <summary>
    /// конфигурация minio
    /// </summary>
    public static class ConfigureMinio
    {
        public static IServiceCollection AddAppMinio(this IServiceCollection services, IConfiguration config)
        {

            var settings = services.AddAppSettings<MinioSettings>(config);

            if (settings.SecretKey != null && settings.AccessKey != null)
            {
                services.AddMinio(new Uri($"http://{settings.AccessKey}:{settings.SecretKey}@{settings.Endpoint}"));
            }

            return services;
        }

        public static void EnsureMinioBucketExists(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var minioClient = scope.ServiceProvider.GetRequiredService<IMinioClient>();
                var minioSettings = scope.ServiceProvider.GetRequiredService<MinioSettings>();

                var beArgs = new BucketExistsArgs().WithBucket(minioSettings.Bucket);
                bool found = minioClient.BucketExistsAsync(beArgs).Result;
                if (!found)
                {
                    try
                    {
                        var mbArgs = new MakeBucketArgs().WithBucket(minioSettings.Bucket);
                        minioClient.MakeBucketAsync(mbArgs).Wait();
                    }
                    catch
                    {
                        throw new Exception("Image storage error");
                    }
                }
            }
        }
    }
}
