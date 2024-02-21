using AutoMapper;
using LearnProject.Domain.Repositories;
using LearnProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LearnProject.BLL.Contracts.Models;
using LearnProject.BLL.Contracts;
using LearnProject.Domain.Models;
using Minio;
using Minio.DataModel.Args;
using Microsoft.AspNetCore.WebUtilities;
using Cars.Api.Settings;
using System.Runtime.InteropServices;

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

        readonly IImageService imageService;

        readonly MinioSettings minioSettings;

        /// <summary>
        /// репозиторий для работы с сущностями
        /// </summary>
        readonly IRepositoryWrapper repository;


        public CarService(IRepositoryWrapper repository,
            IMapper mapper, MinioSettings minioSettings, IImageService imageService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.minioSettings = minioSettings;
            this.imageService = imageService;
        }

        /// <summary>
        /// получить все машины
        /// </summary>
        /// <returns>коллекция авто GetCarModel</returns>
        /// <param name="limit">максимальный размер выборки</param>
        /// <param name="offset">смещение от начала</param>
        public async Task<PagedList<GetCarModel>> GetCarsAsync(CarQueryParameters parameters)
        {
            var cars = repository.CarRepository.GetCars(parameters);

            var carModels = mapper.Map<List<GetCarModel>>(cars);

            await Parallel.ForEachAsync(carModels, async (model, token) => 
                model.Image = (await imageService.GetImageAsync($"car_{model.CarId}")).Value?.ToArray());

            var data = new PagedList<GetCarModel>(carModels, cars.TotalCount, cars.CurrentPage, cars.PageSize);

            return data;
        }

        /// <summary>
        /// получение по id
        /// </summary>
        /// <param name="id">id авто</param>
        /// <returns>модель ответа, содержащая GetCarModel</returns>
        public async Task<ServiceResponse<GetCarModel>> GetCarAsync(int id)
        {
            var car = await repository.CarRepository.FindByKeyAsync(id);
            if (car == null)
            {
                return ServiceResponse<GetCarModel>.CreateFailedResponse($"Car with id {id} not found");
            }

            GetCarModel value = mapper.Map<GetCarModel>(car);

            var serviceResponse = await imageService.GetImageAsync($"car_{car.CarId}");

            if (serviceResponse.IsSuccessful)
            {
                value.Image = serviceResponse.Value!.ToArray();
            }

            return ServiceResponse<GetCarModel>.CreateSuccessfulResponse(value);
        }

        /// <summary>
        /// получить все марки авто
        /// </summary>
        /// <returns>коллекция марок авто</returns>
        public async Task<IEnumerable<GetCarBrandModel>> GetCarBrandModelsAsync()
        {
            IEnumerable<CarModel> models = await repository.CarModelRepository.ReadAll().ToListAsync();

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
            var model = await repository.CarModelRepository.FindByKeyAsync(carModel.CarModelId);

            if (model == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car model with id {carModel.CarModelId} not found");

            Car car = mapper.Map<Car>(carModel);

            repository.CarRepository.Create(car);

            await repository.SaveAsync();

            await imageService.UploadImageAsync($"car_{car.CarId}", carModel.Image);

            GetCarModel value = mapper.Map<GetCarModel>(car);

            return ServiceResponse<int>.CreateSuccessfulResponse(value.CarId);
        }

        /// <summary>
        /// изменение авто
        /// </summary>
        /// <param name="id">id машины</param>
        /// <param name="carModel">модель изменения</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> UpdateCarAsync(int id, UpdateCarModel carModel)
        {
            var car = await repository.CarRepository.FindByKeyAsync(id);
            if (car == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car with id {id} not found");

            var model = await repository.CarModelRepository.FindByKeyAsync(carModel.CarModelId);
            if (model == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car model with id {carModel.CarModelId} not found");

            carModel.CarId = id;
            mapper.Map(carModel, car);

            var response = await imageService.UploadImageAsync($"car_{car.CarId}", carModel.Image);

            if (!response.IsSuccessful)
            {
                return ServiceResponse<int>.CreateFailedResponse(response.Error);
            }

            await repository.SaveAsync();

            return ServiceResponse<int>.CreateSuccessfulResponse();
        }
        
        /// <summary>
        /// удаление машины
        /// </summary>
        /// <param name="id">id машины</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> DeleteCarAsync(int id)
        {
            var car = await repository.CarRepository.FindByKeyAsync(id);
            if (car == null)
                return ServiceResponse<int>.CreateFailedResponse($"Car with id {id} not found");

            repository.CarRepository.Delete(car);

            var response = await imageService.DeleteImageAsync($"car_{car.CarId}");

            if (!response.IsSuccessful)
            {
                return ServiceResponse<int>.CreateFailedResponse(response.Error);
            }

            await repository.SaveAsync();

            return ServiceResponse<int>.CreateSuccessfulResponse();
        }
    }
}
