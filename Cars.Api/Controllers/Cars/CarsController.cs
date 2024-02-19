using AutoMapper;
using Cars.Api.Controllers.Cars.FIlters;
using Cars.Api.Controllers.Cars.Models;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Models;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cars.Api.Controllers.Cars
{
    /// <summary>
    /// контроллер машин
    /// </summary>
    [ApiController]
    [Authorize(Policy = AppPolicies.ViewCars)]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {

        readonly ICarService service;
        readonly IMapper mapper;

        public CarsController(ICarService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        /// <summary>
        /// получение авто
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<CarResponse>), 200)]
        public async Task<IEnumerable<CarResponse>> GetCars([FromQuery] CarQueryParameters parameters)
        {
            var cars = await service.GetCarsAsync(parameters);
            var response = mapper.Map<IEnumerable<CarResponse>>(cars);

            var metadata = new
            {
                cars.TotalCount,
                cars.PageSize,
                cars.CurrentPage,
                cars.TotalPages
            };

            Response.Headers.Append("Pagination", JsonSerializer.Serialize(metadata));

            return response;
        }

        /// <summary>
        /// получение моделей авто
        /// </summary>
        [HttpGet("brandModels")]
        [ProducesResponseType(typeof(IEnumerable<CarBrandModelResponse>), 200)]
        public async Task<IEnumerable<CarBrandModelResponse>> GetCarBrandModels()
        {
            var cars = await service.GetCarBrandModelsAsync();
            var response = mapper.Map<IEnumerable<CarBrandModelResponse>>(cars);

            return response;
        }

        /// <summary>
        /// получение авто по id
        /// </summary>
        /// <param name="id">id машины</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CarResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarResponse>> GetCarById(int id)
        {
            var response = await service.GetCarAsync(id);

            if (!response.IsSuccessful)
            {
                return NotFound();
            }

            var carResponse = mapper.Map<CarResponse>(response.Value);

            return carResponse;
        }

        /// <summary>
        /// попытка добавления машины
        /// </summary>
        [HttpPost("")]
        [Authorize(Policy = AppPolicies.EditCars)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(MinioCheckBucketResourceFilter))]
        public async Task<ActionResult<int>> AddCar(AddCarRequest request)
        {
            var model = mapper.Map<AddCarModel>(request);

            model.Image = request.Image.OpenReadStream();

            ServiceResponse<int> response = await service.AddCarAsync(model);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Error);
            }

            var carResponse = response.Value;

            return carResponse;
        }

        /// <summary>
        /// попытка редактирования
        /// </summary>
        /// <param name="id">id машины</param>
        /// <param name="request">модель запроса</param>
        [HttpPut("{id}")]
        [Authorize(Policy = AppPolicies.EditCars)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCar(int id, UpdateCarRequest request)
        {
            var model = mapper.Map<UpdateCarModel>(request);

            ServiceResponse<int> response = await service.UpdateCarAsync(id, model);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Error);
            }

            return NoContent();
        }

        /// <summary>
        /// попытка удаления
        /// </summary>
        /// <param name="id">id машины</param>
        [HttpDelete("{id}")]
        [Authorize(Policy = AppPolicies.EditCars)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCar(int id)
        {
            ServiceResponse<int> response = await service.DeleteCarAsync(id);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Error);
            }

            return NoContent();
        }
    }
}
