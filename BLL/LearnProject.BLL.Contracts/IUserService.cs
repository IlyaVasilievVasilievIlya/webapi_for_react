using LearnProject.BLL.Contracts.Models;

namespace LearnProject.BLL.Contracts
{
    public interface IUserService
    {
        /// <summary>
        /// получить всех пользователей
        /// </summary>
        /// <returns>коллекция пользователей GetUserModel</returns>
        /// <param name="limit">максимальный размер выборки</param>
        /// <param name="offset">смещение от начала</param>
        Task<IEnumerable<GetUserModel>> GetUsersAsync(int offset = 0, int limit = 1000);

        /// <summary>
        /// получить роли пользователей
        /// </summary>
        /// <returns>коллекция ролей string</returns>
        Task<IEnumerable<string>> GetRolesAsync();

        /// <summary>
        /// получение пользователя по id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>модель ответа, содержащая GetUserModel</returns>
        Task<ServiceResponse<GetUserModel>> GetUserAsync(string id);

        /// <summary>
        /// получение пользователя по username
        /// </summary>
        /// <param name="id">имя пользователя</param>
        /// <returns>модель ответа, содержащая GetUserModel</returns>
        Task<ServiceResponse<GetUserModel>> GetUserByNameAsync(string userName);

        /// <summary>
        /// существует ли пользователь в бд
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <returns>результат проверки</returns>
        Task<bool> IsUserFound(string login, string password);

        /// <summary>
        /// получить общее число пользователей
        /// </summary>
        /// <returns>число пользователей</returns>
        Task<int> GetUsersCountAsync();

        /// <summary>
        /// сменить роль пользователя
        /// </summary>
        Task<ServiceResponse<int>> ChangeRoleAsync(string id, string newRole);

        /// <summary>
        /// изменение пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="model">модель пользователя</param>
        /// <returns>модель ответа</returns>
        Task<ServiceResponse<int>> UpdateUserAsync(string id, UpdateUserModel model);
    }
}
