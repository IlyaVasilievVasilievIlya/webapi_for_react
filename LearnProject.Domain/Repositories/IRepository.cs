using System.Linq.Expressions;

namespace LearnProject.Domain.Repositories
{
    /// <summary>
    /// контракт репозитория
    /// </summary>
    /// <typeparam name="T">тип сущности</typeparam>
    /// <typeparam name="TKey">PK сущности</typeparam>
    public interface IRepository<T, TKey>
    {
        /// <summary>
        /// получить сущности
        /// </summary>
        /// <returns>коллекция сущностей</returns>
        IQueryable<T> ReadAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        Task<T?> FindByKeyAsync(TKey key);

        /// <summary>
        /// создать сущность
        /// </summary>
        /// <param name="value">сущность</param>
        void Create(T value);

        /// <summary>
        /// изменить сущность
        /// </summary>
        /// <param name="value">сущность</param>
        void Update(T value);

        /// <summary>
        /// удаление сущности
        /// </summary>
        /// <param name="value">сущность</param>
        void Delete(T value);
    }
}
