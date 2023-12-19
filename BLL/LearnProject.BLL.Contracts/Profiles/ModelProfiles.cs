using AutoMapper;
using LearnProject.Domain.Entities;
using LearnProject.Domain.Models;

namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// профиль маппинга марки (сущность -> модель)
    /// </summary>
    public class CarBrandModelProfile : Profile
    {
        public CarBrandModelProfile()
        {
            CreateMap<CarModel, GetCarBrandModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(
                    src => string.Format("{0} {1}", src.Brand, src.Name)));
        }
    }

    /// <summary>
    /// профиль маппинга (POCO -> модель сервиса)
    /// </summary>
    public class CarModelProfile : Profile
    {
        public CarModelProfile()
        {
            CreateMap<Car, GetCarModel>()
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.CarModel));
        }
    }

    /// <summary>
    /// профиль маппинга (модель добавления -> сущность)
    /// </summary>
    public class AddCarModelProfile : Profile
    {
        public AddCarModelProfile()
        {
            CreateMap<AddCarModel, Car>();
        }
    }

    /// <summary>
    /// профиль маппинга (модель редактирования -> сущность)
    /// </summary>
    public class UpdateCarModelProfile : Profile
    {
        public UpdateCarModelProfile()
        {
            CreateMap<UpdateCarModel, Car>();
        }
    }

    /// <summary>
    /// профиль маппинга (модель добавления -> сущность)
    /// </summary>
    public class AddUserModelProfile : Profile
    {
        public AddUserModelProfile()
        {
            CreateMap<RegisterUserModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }

    /// <summary>
    /// профиль маппинга (модель сущности с ролью -> модель сервиса)
    /// </summary>
    public class GetUserModelProfile : Profile
    {
        public GetUserModelProfile()
        {
            CreateMap<UserWithRoleModel, GetUserModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname))
                .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.User.Patronymic))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User.BirthDate))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}
