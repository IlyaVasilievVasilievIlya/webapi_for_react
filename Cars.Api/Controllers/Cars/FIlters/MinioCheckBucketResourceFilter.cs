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

        public MinioCheckBucketResourceFilter(IMinioClient minioClient, MinioSettings minioSettings)
        {
            this.minioClient = minioClient;
            this.minioSettings = minioSettings;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            //var client = new MinioClient().WithEndpoint(minioSettings.Endpoint)
            //    .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
            //    .Build();

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
