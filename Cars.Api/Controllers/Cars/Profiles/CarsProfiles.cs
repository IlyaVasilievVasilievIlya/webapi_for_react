using AutoMapper;
using Cars.Api.Controllers.Cars.Models;
using LearnProject.BLL.Contracts.Models;

namespace Cars.Api.Controllers.Cars.Profiles
{
    /// <summary>
    /// маппинг (модель машины из сервиса  -> модель списка ответа )
    /// </summary>
    public class CarResponseProfile : Profile
    {
        public CarResponseProfile()
        {
            CreateMap<GetCarModel, CarResponse>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Model));
        }
    }

    /// <summary>
    /// маппинг (модель запроса на добавление -> модель добавления из сервиса)
    /// </summary>
    public class AddCarRequestProfile : Profile
    {
        public AddCarRequestProfile()
        {
            CreateMap<AddCarRequest, AddCarModel>().ForMember(x => x.Image, opt => opt.Ignore()); 
        }
    }

    /// <summary>
    /// маппинг (модель запроса на редактирование -> модель редактирования сервиса)
    /// </summary>
    public class UpdateCarRequestProfile : Profile
    {
        public UpdateCarRequestProfile()
        {
            CreateMap<UpdateCarRequest, UpdateCarModel>();
        }
    }

    /// <summary>
    /// маппинг (модель бренда машины -> модель ответа)
    /// </summary>
    public class CarBrandModelResponseProfile : Profile
    {
        public CarBrandModelResponseProfile()
        {
            CreateMap<GetCarBrandModel, CarBrandModelResponse>();
        }
    }
}
