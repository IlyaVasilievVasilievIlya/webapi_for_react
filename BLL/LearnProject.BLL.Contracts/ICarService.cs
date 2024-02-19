using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Models;

namespace LearnProject.BLL.Contracts
{
    /// <summary>
    /// контракт работы с сущностями машин
    /// </summary>
    public interface ICarService
    {
        /// <summary>
        /// получить все машины
        /// </summary>
        /// <returns>коллекция авто GetCarModel</returns>
        /// <param name="limit">максимальный размер выборки</param>
        /// <param name="offset">смещение от начала</param>
        Task<PagedList<GetCarModel>> GetCarsAsync(CarQueryParameters parameters);

        /// <summary>
        /// получение по id
        /// </summary>
        /// <param name="id">id авто</param>
        /// <returns>модель ответа, содержащая GetCarModel</returns>
        Task<ServiceResponse<GetCarModel>> GetCarAsync(int id);

        /// <summary>
        /// получить все марки авто
        /// </summary>
        /// <returns>коллекция марок авто</returns>
        Task<IEnumerable<GetCarBrandModel>> GetCarBrandModelsAsync();

        /// <summary>
        /// добавление машины
        /// </summary>
        /// <param name="carModel">модель для добавления</param>
        /// <returns>модель ответа</returns>
        Task<ServiceResponse<int>> AddCarAsync(AddCarModel car);

        /// <summary>
        /// изменение авто
        /// </summary>
        /// <param name="id">id машины</param>
        /// <param name="carModel">модель изменения</param>
        /// <returns>модель ответа</returns>
        Task<ServiceResponse<int>> UpdateCarAsync(int id, UpdateCarModel car);

        /// <summary>
        /// удаление машины
        /// </summary>
        /// <param name="id">id машины</param>
        /// <returns>модель ответа</returns>
        Task<ServiceResponse<int>> DeleteCarAsync(int id);
    }
}
