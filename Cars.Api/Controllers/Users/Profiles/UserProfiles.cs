﻿using AutoMapper;
using Cars.Api.Controllers.Users.Models;
using LearnProject.BLL.Contracts.Models;

namespace Cars.Api.Controllers.Users.Profiles
{
    public class UserProfiles
    {
        /// <summary>
        /// маппинг (модель пользователя из сервиса  -> модель ответа клиенту)
        /// </summary>
        public class UserResponseProfile : Profile
        {
            public UserResponseProfile()
            {
                CreateMap<GetUserModel, UserResponse>()
                    .ForMember(dest => dest.FullName, opt => 
                        opt.MapFrom(src => string.Format("{0} {1}{2}", src.Surname, 
                            src.Name, (src.Patronymic != null) ? $" {src.Patronymic}": "" )));
            }
        }

        /// <summary>
        /// маппинг (модель пользователя из сервиса  -> клиентская модель запроса на изменение)
        /// (клиентская модель запроса на изменение  -> модель запроса к сервису)
        /// </summary>
        public class UpdateUserRequestProfile : Profile
        {
            public UpdateUserRequestProfile()
            {
                CreateMap<GetUserModel, UpdateUserRequest>();
                CreateMap<UpdateUserRequest, UpdateUserModel>();
            }
        }
    }
}