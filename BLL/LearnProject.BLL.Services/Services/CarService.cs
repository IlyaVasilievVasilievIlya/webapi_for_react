using AutoMapper;
using LearnProject.Domain.Repositories;
using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LearnProject.BLL.Contracts.Models;
using LearnProject.BLL.Contracts;

namespace LearnProject.BLL.Services
{
    /// <summary>
    /// сервис работы с сущностями авто в БД
    /// </summary>
    public sealed class CarService : ICarService
    {
        /// <summary>
        /// маппер (POCO <-> модель сервиса)
        /// </summary>
        readonly IMapper mapper;

        /// <summary>
        /// репозиторий для работы с сущностями
        /// </summary>
        readonly ICarRepository carRepository;

        /// <summary>
        /// репозиторий для работы с сущностями моделей авто
        /// </summary>
        readonly ICarModelRepository modelRepository;

        public CarService(ICarRepository carRepository, ICarModelRepository modelRepository,
            IMapper mapper)
        {
            this.carRepository = carRepository;
            this.modelRepository = modelRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// получить все машины
        /// </summary>
        /// <returns>коллекция авто GetCarModel</returns>
        /// <param name="limit">максимальный размер выборки</param>
        /// <param name="offset">смещение от начала</param>
        public async Task<IEnumerable<GetCarModel>> GetCarsAsync(int offset = 0, int limit = 1000)
        {
            var cars = await carRepository.ReadAllAsync(offset, limit);
            var data = mapper.Map<List<GetCarModel>>(cars);

            return data;
        }

        /// <summary>
        /// получение по id
        /// </summary>
        /// <param name="id">id авто</param>
        /// <returns>модель ответа, содержащая GetCarModel</returns>
        public async Task<ServiceResponse<GetCarModel>> GetCarAsync(int id)
        {
            var car = await carRepository.ReadAsync(id);
            if (car == null)
            {
                return ServiceResponse<GetCarModel>.CreateFailedResponse($"Car with id {id} not found");
            }

            GetCarModel value = mapper.Map<GetCarModel>(car);

            return ServiceResponse<GetCarModel>.CreateSuccessfulResponse(value);
        }

        /// <summary>
        /// получить число машин в базе
        /// </summary>
        /// <returns>число машин</returns>
        public async Task<int> GetCarsCountAsync() => await carRepository.Query(cars => cars.Skip(1).CountAsync());

        /// <summary>
        /// получить все марки авто
        /// </summary>
        /// <returns>коллекция марок авто</returns>
        public async Task<IEnumerable<GetCarBrandModel>> GetCarBrandModelsAsync()
        {
            IEnumerable<CarModel> models = await modelRepository.ReadAllAsync();

            var data = mapper.Map<List<GetCarBrandModel>>(models);

            return data;
        }

        /// <summary>
        /// добавление машины
        /// </summary>
        /// <param name="carModel">модель для добавления</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> AddCarAsync(AddCarModel carModel)
        {
            var model = await modelRepository.ReadAsync(carModel.CarModelId);

            if (model == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car model with id {carModel.CarModelId} not found");

            Car car = mapper.Map<Car>(carModel);

            await carRepository.CreateAsync(car);

            return ServiceResponse<int>.CreateSuccessfulResponse();
        }

        /// <summary>
        /// изменение авто
        /// </summary>
        /// <param name="id">id машины</param>
        /// <param name="carModel">модель изменения</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> UpdateCarAsync(int id, UpdateCarModel carModel)
        {
            var car = await carRepository.ReadAsync(id);
            if (car == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car with id {id} not found");

            var model = await modelRepository.ReadAsync(carModel.CarModelId);
            if (model == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car model with id {carModel.CarModelId} not found");

            carModel.CarId = id;
            mapper.Map(carModel, car);

            await carRepository.UpdateAsync(car);

            return ServiceResponse<int>.CreateSuccessfulResponse();
        }
        
        /// <summary>
        /// удаление машины
        /// </summary>
        /// <param name="id">id машины</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> DeleteCarAsync(int id)
        {
            var car = await carRepository.ReadAsync(id);
            if (car == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car with id {id} not found");

            await carRepository.DeleteAsync(car);

            return ServiceResponse<int>.CreateSuccessfulResponse();
        }
    }
}
