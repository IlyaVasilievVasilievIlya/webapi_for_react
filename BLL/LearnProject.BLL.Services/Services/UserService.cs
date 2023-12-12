using AutoMapper;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
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
        readonly IUserRepository userRepository;
        readonly IMapper mapper;

        public UserService(UserManager<User> userManager,
            IMapper mapper, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// получить пользователей с их ролями
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. кол-во</param>
        /// <returns>коллекцию моделей пользователей</returns>
        public async Task<IEnumerable<GetUserModel>> GetUsersAsync(int offset = 0, int limit = 1000)
        {
            var users = await userRepository.ReadAllWithRolesAsync(offset, limit);

            var data = mapper.Map<List<GetUserModel>>(users);

            return data;
        }

        /// <summary>
        /// получить пользователя с ролью
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>модель ответа с моделью пользователя</returns>
        public async Task<ServiceResponse<GetUserModel>> GetUserAsync(string id)
        {
            var user = await userRepository.ReadAsync(id);
            if (user == null)
            {
                return ServiceResponse<GetUserModel>.CreateFailedResponse($"User with id {id} not found");
            }

            string userRole = (await userManager.GetRolesAsync(user)).First();

            GetUserModel value = new GetUserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                BirthDate = user.BirthDate,
                Role = userRole
            };

            return ServiceResponse<GetUserModel>.CreateSuccessfulResponse(value);
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

            GetUserModel value = new GetUserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                BirthDate = user.BirthDate,
                Role = userRole
            };

            return ServiceResponse<GetUserModel>.CreateSuccessfulResponse(value);
        }

        /// <summary>
        /// существует ли пользователь в бд
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <returns>результат проверки</returns>
        public async Task<bool> IsUserFound(string login, string password)
        {
            var user = await userManager.FindByNameAsync(login);
            if (user == null)
            {
                return false;
            }

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// получить все роли
        /// </summary>
        /// <returns>коллекция ролей</returns>
        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            return await userRepository.ReadAllRolesAsync();
        }

        /// <summary>
        /// найти количество пользователей
        /// </summary>
        /// <returns>количество пользователей</returns>
        public async Task<int> GetUsersCountAsync()
        {
            return await userRepository.Query(users => users.Skip(1).CountAsync());
        }

        /// <summary>
        /// изменение пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="model">модель пользователя</param>
        /// <returns>модель ответа</returns>
        public async Task<ServiceResponse<int>> UpdateUserAsync(string id, UpdateUserModel model)
        {
            var user = await userRepository.ReadAsync(id);
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
            var user = await userRepository.ReadAsync(id);

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
