using AutoMapper;
using Cars.Api.Controllers.Identity.Models;
using LearnProject.BLL.Contracts.Models;

namespace Cars.Api.Controllers.Identity.Profiles
{
    /// <summary>
    /// профили маппинга сервиса аутентификации
    /// </summary>
    public class IdentityProfiles
    {
        /// <summary>
        /// маппинг (модель запроса на логин  -> модель запроса для сервиса)
        /// </summary>
        public class LoginRequestProfile : Profile
        {
            public LoginRequestProfile()
            {
                CreateMap<LoginRequest, LoginUserModel>();
            }
        }

        /// <summary>
        /// модель регистрации приложения -> сервиса
        /// </summary>
        public class RegisterRequestProfile : Profile
        {
            public RegisterRequestProfile()
            {
                CreateMap<RegisterRequest, RegisterUserModel>();
            }
        }
    }
}
