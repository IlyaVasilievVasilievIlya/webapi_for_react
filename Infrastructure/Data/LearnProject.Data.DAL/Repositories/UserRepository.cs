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
        /// получение пользователей с их ролями
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. количество</param>
        /// <returns>коллекция моделей UserWithRoleModel</returns>
        public PagedList<UserWithRoleModel> ReadAllWithRoles(UserQueryParameters parameters)
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

            return PagedList<UserWithRoleModel>.ToPagedList(result.OrderBy(e => e.User.Id), parameters.PageNumber, parameters.PageSize);
        }

        public override Task<User?> FindByKeyAsync(string key)
        {
            return FindByCondition(entity => entity.Id == key).AsTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// реализация запроса на получение всех ролей
        /// </summary>
        /// <returns>коллекция ролей string</returns>
        public IQueryable<string> ReadAllRoles()
        {
            var roles = context.Roles
                .AsNoTracking()
                .Select(role => role.Name ?? string.Empty)
                .Where(role => role != AppRoles.SuperUser);

            return roles;
        }

        /// <summary>
        /// получить пользователя с ролью
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>модель ответа</returns>
        public async Task<UserWithRoleModel> ReadWithRoleAsync(string id)
        {
            var user = await FindByKeyAsync(id);

            if (user == null)
            {
                throw new Exception("role not found");
            }

            var role = context.UserRoles.AsQueryable().Where(userRole => userRole.UserId == id)
                .Join(context.Roles.AsQueryable(), user => user.RoleId, role => role.Id,
                    (user, role) => role.Name ?? "" ).First();

            return new UserWithRoleModel() { User = user, Role = role};
        }

        public Task<RefreshToken> ReadRefreshTokenAsync(string refreshToken)
        {
            var tokens = context.RefreshTokens.Include(e => e.User);
            return tokens.Where(token => token.Token == refreshToken).FirstAsync();
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var oldToken = await context.RefreshTokens.Where(token => token.UserId == refreshToken.UserId).FirstOrDefaultAsync();
            if (oldToken != null)
            {
                context.RefreshTokens.Remove(oldToken);
            }
            context.RefreshTokens.Add(refreshToken);
        }

        public void DeleteRefreshTokenAsync(RefreshToken refreshToken)
        {
            context.RefreshTokens.Remove(refreshToken);
        }
    }
}
