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
        /// <summary>
        /// получить изображение
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResponse<MemoryStream>> GetImageAsync(string id);

        /// <summary>
        /// загрузить / обновить картинку
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<ServiceResponse<int>> UploadImageAsync(string id, Stream stream);

        /// <summary>
        /// удалить картинку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResponse<int>> DeleteImageAsync(string id);
    }
}
