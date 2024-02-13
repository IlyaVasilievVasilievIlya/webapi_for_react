using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Models;

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
        PagedList<GetUserModel> GetUsers(UserQueryParameters parameters);

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
