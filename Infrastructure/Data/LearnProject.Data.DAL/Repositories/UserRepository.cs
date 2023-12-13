using LearnProject.Domain.Entities;
using LearnProject.Domain.Models;
using LearnProject.Domain.Repositories;
using LearnProject.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data.DAL.Repositories
{
    /// <summary>
    /// реализация репозитория работы с пользователями
    /// </summary>
    public sealed class UserRepository : AbstractRepository<User, string>, IUserRepository
    {
        /// <summary>
        /// контекст БД
        /// </summary>
        RepositoryAppDbContext context;

        public UserRepository(RepositoryAppDbContext context)
        {
            this.context = context;
        }

        protected override RepositoryAppDbContext Context
        {
            get => context;
            set => context = value;
        }

        /// <summary>
        /// реализация запроса по умолчанию
        /// </summary>
        /// <returns>коллекция сущностей User</returns>
        protected override IQueryable<User> QueryImplementation()
        {
            return context.Users.AsNoTracking();
        }

        /// <summary>
        /// реализация запроса всех пользователей
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. количество</param>
        /// <returns>коллекция сущностей User</returns>
        protected override async Task<IEnumerable<User>> ReadAllImplementationAsync(int offset, int limit)
        {
            var users = context.Users
                .AsNoTracking();

            users = users.OrderBy(user => user.Id)
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, limit));

            return await users.ToListAsync();
        }

        /// <summary>
        /// получение пользователей с их ролями
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. количество</param>
        /// <returns>коллекция моделей UserWithRoleModel</returns>
        public async Task<IEnumerable<UserWithRoleModel>> ReadAllWithRolesAsync(int offset = 0, int limit = 1000)
        {
            var userRolesIds = context.UserRoles
                .AsQueryable()
                .GroupBy(t => t.UserId)
                .Select(g => new { Id = g.Key, g.Single().RoleId });

            var result = context.Users
                .AsQueryable()
                .Join(userRolesIds, user => user.Id, ur => ur.Id,
                    (user, userRole) => new { User = user, userRole.RoleId })
                .Join(context.Roles.AsQueryable(), user => user.RoleId, role => role.Id,
                    (user, role) => new UserWithRoleModel { User = user.User, Role = role.Name ?? "" });

            result = result.AsNoTracking()
                .Where(user => user.Role != AppRoles.SuperUser);

            result = result.OrderBy(user => user.User.Id)
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, limit));

            return await result.ToListAsync();
        }

        /// <summary>
        /// реализация запроса на получение пользователя по id
        /// </summary>
        /// <param name="key">id пользователя</param>
        /// <returns>сущность пользователя либо null</returns>
        protected override async Task<User?> ReadImplementationAsync(string key) => await context.Users.FindAsync(key);

        /// <summary>
        /// реализация запроса на получение всех ролей
        /// </summary>
        /// <returns>коллекция ролей string</returns>
        public async Task<IEnumerable<string>> ReadAllRolesAsync()
        {
            var roles = context.Roles
                .AsNoTracking()
                .Select(role => role.Name ?? string.Empty)
                .Where(role => role != AppRoles.SuperUser);

            return await roles.ToListAsync();
        }

        /// <summary>
        /// получить пользователя с ролью
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>модель ответа</returns>
        public async Task<UserWithRoleModel> ReadWithRoleAsync(string id)
        {
            var result = context.Users
                .AsQueryable()
                .Where(user => user.Id == id)
                .Join(context.UserRoles.AsQueryable(), user => user.Id, ur => ur.UserId,
                    (user, userRole) => new { User = user, userRole.RoleId })
                .Join(context.Roles.AsQueryable(), user => user.RoleId, role => role.Id,
                    (user, role) => new UserWithRoleModel { User = user.User, Role = role.Name ?? "" });

            return await result.FirstAsync();
        }

        /// <summary>
        /// реализация запроса на создание
        /// </summary>
        /// <param name="user">сущность пользователя</param>
        protected override void CreateImplementation(User user) => context.Users.Add(user);

        /// <summary>
        /// реализация запроса на редактирование
        /// </summary>
        /// <param name="user">сущность пользователя</param>
        protected override void UpdateImplementation(User user) => context.Users.Update(user);

        /// <summary>
        /// реализация запроса на удаление
        /// </summary>
        /// <param name="user">сущность пользователя</param>
        protected override void DeleteImplementation(User user) => context.Users.Remove(user);

        public async Task<RefreshToken?> ReadRefreshTokenAsync(string refreshToken)
        {
            var tokens = context.RefreshTokens.Include(e => e.User);
            return await tokens.Where(token => token.Token == refreshToken).FirstOrDefaultAsync();
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var oldToken = context.RefreshTokens.Where(token => token.UserId == refreshToken.UserId).FirstOrDefault();
            if (oldToken != null)
            {
                context.RefreshTokens.Remove(oldToken);
            }
            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();
        }
    }
}
