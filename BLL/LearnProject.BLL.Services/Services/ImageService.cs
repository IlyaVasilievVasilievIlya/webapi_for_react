using Cars.Api.Settings;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using Minio.DataModel.Args;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace LearnProject.BLL.Services.Services
{
    public class ImageService : IImageService
    {
        readonly IMinioClient minioClient;
        readonly MinioSettings settings;

        public ImageService(IMinioClient minioClient, MinioSettings settings)
        {
            this.minioClient = minioClient;
            this.settings = settings;
        }

        public async Task<ServiceResponse<int>> DeleteImageAsync(string id)
        {
            try
            {
                await CheckObjectExists(id);

                await minioClient.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(settings.Bucket).WithObject(id));
                return ServiceResponse<int>.CreateSuccessfulResponse();
            }
            catch (Exception ex)
            {
                return ServiceResponse<int>.CreateFailedResponse(ex.Message);
            }
        }

        public async Task<ServiceResponse<MemoryStream>> GetImageAsync(string id)
        {
            try
            {
                await CheckObjectExists(id);

                MemoryStream stream = new MemoryStream();
                var streamd = await minioClient.GetObjectAsync(new GetObjectArgs().WithBucket(settings.Bucket).WithObject(id).
                    WithCallbackStream(str => str.CopyTo(stream)));
                return ServiceResponse<MemoryStream>.CreateSuccessfulResponse(stream);
            }
            catch (Exception ex)
            {
                return ServiceResponse<MemoryStream>.CreateFailedResponse(ex.Message);
            }
        }

        public async Task<ServiceResponse<int>> UploadImageAsync(string id, Stream stream)
        {
            try
            {
                await minioClient.PutObjectAsync(new PutObjectArgs()
                .WithStreamData(stream)
                .WithBucket(settings.Bucket)
                .WithObject(id).WithObjectSize(stream.Length));

                await CheckObjectExists(id);

                return ServiceResponse<int>.CreateSuccessfulResponse();
            }
            catch (Exception ex)
            {
                return ServiceResponse<int>.CreateFailedResponse(ex.Message);
            }
        }

        async Task CheckObjectExists(string id)
        {
            StatObjectArgs args = new StatObjectArgs()
                .WithBucket(settings.Bucket).WithObject(id);
            await minioClient.StatObjectAsync(args);
        }
    }
}
