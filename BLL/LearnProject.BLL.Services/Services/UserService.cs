using AutoMapper;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using LearnProject.Domain.Models;
using LearnProject.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.BLL.Services
{
    /// <summary>
    /// сервис для работы с пользователями
    /// </summary>
    public sealed class UserService : IUserService
    {
        readonly UserManager<User> userManager;
        readonly IRepositoryWrapper repository;
        readonly IMapper mapper;

        public UserService(UserManager<User> userManager,
            IMapper mapper, IRepositoryWrapper repository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// получить пользователей с их ролями
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. кол-во</param>
        /// <returns>коллекцию моделей пользователей</returns>
        public PagedList<GetUserModel> GetUsers(UserQueryParameters parameters)
        {
            var users = repository.UserRepository.ReadAllWithRoles(parameters);

            var data = new PagedList<GetUserModel>(mapper.Map<List<GetUserModel>>(users), users.TotalCount, users.CurrentPage, users.PageSize);

            return data;
        }

        /// <summary>
        /// получить пользователя с ролью
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>модель ответа с моделью пользователя</returns>
        public async Task<ServiceResponse<GetUserModel>> GetUserAsync(string id)
        {
            var user = await repository.UserRepository.FindByKeyAsync(id);
            if (user == null)
            {
                return ServiceResponse<GetUserModel>.CreateFailedResponse($"User with id {id} not found");
            }

            string userRole = (await userManager.GetRolesAsync(user)).First();

            GetUserModel userModel = mapper.Map<GetUserModel>(user);

            userModel.Role = userRole;


            return ServiceResponse<GetUserModel>.CreateSuccessfulResponse(userModel);
        }

        /// <summary>
        /// получить пользователя по username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<GetUserModel>> GetUserByNameAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return ServiceResponse<GetUserModel>.CreateFailedResponse($"User with name {userName} not found");
            }

            string userRole = (await userManager.GetRolesAsync(user)).First();

            GetUserModel userModel = mapper.Map<GetUserModel>(user);

            userModel.Role = userRole;

            return ServiceResponse<GetUserModel>.CreateSuccessfulResponse(userModel);
        }

        /// <summary>
        /// получить все роли
        /// </summary>
        /// <returns>коллекция ролей</returns>
        public  Task<List<string>> GetRolesAsync()
        {
            return repository.UserRepository.ReadAllRoles().ToListAsync();
        }

        /// <summary>
        /// изменение пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="model">модель пользователя</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> UpdateUserAsync(string id, UpdateUserModel model)
        {
            var user = await repository.UserRepository.FindByKeyAsync(id);
            if (user == null)
                return ServiceResponse<int>.CreateFailedResponse($"User with id {id} not found");

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Patronymic = model.Patronymic;
            user.BirthDate = model.BirthDate;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return ServiceResponse<int>.CreateSuccessfulResponse();
            }
            return ServiceResponse<int>.CreateFailedResponse("Cannot update user");
        }

        /// <summary>
        /// сменить роль пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="newRole">новая роль</param>
        /// <returns>содель результата</returns>
        public async Task<ServiceResponse<int>> ChangeRoleAsync(string id, string newRole)
        {
            var user = await repository.UserRepository.FindByKeyAsync(id);

            if (user == null)
            {
                return ServiceResponse<int>.CreateFailedResponse("Cannot find user");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                return ServiceResponse<int>.CreateFailedResponse("Cannot change user role");
            }

            var addResult = await userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                await userManager.AddToRolesAsync(user, userRoles);
                return ServiceResponse<int>.CreateFailedResponse("Cannot change user role");
            }

            return ServiceResponse<int>.CreateSuccessfulResponse();
        }
    }
}
