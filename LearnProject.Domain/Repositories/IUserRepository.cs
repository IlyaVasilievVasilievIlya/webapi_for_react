using LearnProject.Domain.Entities;
using LearnProject.Domain.Models;

namespace LearnProject.Domain.Repositories
{
    /// <summary>
    /// репозиторий пользователей
    /// </summary>
    public interface IUserRepository : IRepository<User, string>
    {
        /// <summary>
        /// получить сущности c ролями
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. количество</param>
        /// <returns>коллекция сущностей</returns>
        PagedList<UserWithRoleModel> ReadAllWithRoles(UserQueryParameters parameters);

        /// <summary>
        /// получить все роли пользователей
        /// </summary>
        /// <returns>коллекция ролей string</returns>
        IQueryable<string> ReadAllRoles();

        /// <summary>
        /// получить пользователя с ролью
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>модель ответа</returns>
        Task<UserWithRoleModel> ReadWithRoleAsync(string id);

        string GetUserRole(string id);

        /// <summary>
        /// получить refresh токен
        /// </summary>
        /// <param name="refreshToken">токен</param>
        /// <returns>сущность токена</returns>
        Task<RefreshToken> ReadRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// добавить refresh токен
        /// </summary>
        /// <param name="refreshToken">токен</param>
        Task AddRefreshTokenAsync(RefreshToken refreshToken);

        /// <summary>
        /// удаление refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        void DeleteRefreshTokenAsync(RefreshToken refreshToken);
    }
}
