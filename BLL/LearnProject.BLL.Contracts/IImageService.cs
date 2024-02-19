using LearnProject.BLL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.BLL.Contracts
{
    public interface IImageService
    {
        Task<ServiceResponse<int>> UploadImageAsync(string id, Stream stream);

        Task<ServiceResponse<MemoryStream>> GetImageAsync(string id);

        Task<ServiceResponse<int>> DeleteImageAsync(string id);
    }
}
