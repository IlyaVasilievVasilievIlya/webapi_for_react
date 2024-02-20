using Microsoft.AspNetCore.Mvc.Filters;
using Minio.DataModel.Args;
using Minio;
using Cars.Api.Settings;

namespace Cars.Api.Controllers.Cars.FIlters
{
    public class MinioCheckBucketResourceFilter : Attribute, IAsyncResourceFilter
    {
        readonly IMinioClient minioClient;
        readonly MinioSettings minioSettings;
        readonly ILogger<MinioCheckBucketResourceFilter> logger;

        public MinioCheckBucketResourceFilter(IMinioClient minioClient, MinioSettings minioSettings, ILogger<MinioCheckBucketResourceFilter> logger)
        {
            this.minioClient = minioClient;
            this.minioSettings = minioSettings;
            this.logger = logger;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var res = await minioClient.ListBucketsAsync();
            if (res == null)
                logger.LogInformation("res null");
            if (res?.Buckets == null)
                logger.LogInformation("buckets null");
            logger.LogInformation(res?.Buckets[0].Name);
            var beArgs = new BucketExistsArgs().WithBucket(minioSettings.Bucket);
            bool found = await minioClient.BucketExistsAsync(beArgs);
            if (!found)
            {
                try
                {
                    var mbArgs = new MakeBucketArgs().WithBucket(minioSettings.Bucket);
                    await minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);

                }
                catch 
                {
                    throw new Exception("Image storage error");
                }
            }
            await next();
        }
    }
}
